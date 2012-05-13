﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using RiaLibrary.Web;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models.WebService;
using System.IO;

namespace Neptuo.Forms.Web.Controllers.WebService
{
    public class FormController : BaseController
    {
        private static Dictionary<string, HttpPostedFileBase> files = new Dictionary<string, HttpPostedFileBase>();

        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Dependency]
        public IFormDataService DataService { get; set; }

        [Dependency]
        public IFileStorage FileStorage { get; set; }

        [Url("ws/{formPublicIdentifier}/definition")]
        public object GetDefinition(string formPublicIdentifier)
        {
            FormDefinition form = FormService.Get(formPublicIdentifier);
            if (form == null)
                return new HttpStatusCodeResult(404);

            return JsonP(new FormDefinitionModel
            {
                PublicIdentifier = formPublicIdentifier,
                PublicContent = form.PublicContent,
                Type = FormType.GetTypes().FirstOrDefault(t => t.Key == form.FormType).Value,
                Fields = form.Fields.Select(f => new FieldDefinitionModel
                {
                    Name = f.Name,
                    PublicIdentifier = f.PublicIdentifier,
                    Required = f.Required,
                    Type = FieldType.GetTypes().FirstOrDefault(t => t.Key == f.FieldType).Value,
                    FileIdentifier = f.FieldType == FieldType.FileField ? Guid.NewGuid().ToString() : null,
                    TargetFormPublicIdentifier = f.ReferenceFormID != null ? f.FormDefinition.PublicIdentifier : null,
                    TargetFieldPublicIdentifier = f.ReferenceDisplayFieldID != null ? f.ReferenceDisplayField.PublicIdentifier : null
                })
            });
        }

        [Url("ws/{formPublicIdentifier}/data")]
        public ActionResult GetFormData(string formPublicIdentifier, int pageSize = 20, int pageIndex = 0)
        {
            //TODO: Ordering,filtering,column selection
            FormDefinition form = FormService.Get(formPublicIdentifier);
            if (form == null)
                return new HttpStatusCodeResult(404);

            IEnumerable<FormData> formData = DataService.GetList(form.ID).OrderByDescending(d => d.Created).Skip(pageSize * pageIndex).Take(pageSize).ToArray();
            return JsonP(formData.Select(d => new FormListDataModel {
                Created = d.Created,
                PublicIdentifier = d.PublicIdentifier,
                Fields = d.Fields.Select(f => new FieldListDataModel {
                    PublicIdentifier = f.FieldDefinition.PublicIdentifier,
                    Name = f.FieldDefinition.Name,
                    Value = f.GetDisplayValue()
                })
            }));
        }

        [Url("ws/{formPublicIdentifier}/upload")]
        [HttpPost]
        public ActionResult UploadFiles(string formPublicIdentifier)
        {
            foreach (string key in Request.Files.AllKeys)
            {
                if (files.ContainsKey(key))
                    files[key] = Request.Files[key];
                else
                    files.Add(key, Request.Files[key]);
            }

            return new EmptyResult();
        }

        [Url("ws/{formPublicIdentifier}/inquiry-data")]
        public ActionResult GetInquiryData(string formPublicIdentifier)
        {
            FormDefinition form = FormService.Get(formPublicIdentifier);
            if (form == null)
                return new HttpStatusCodeResult(404);

            //TODO: Compute inquiry data! (Use cache?)
            return View();
        }

        [HttpGet]
        [Url("ws/{formPublicIdentifier}/insert")]
        public ActionResult InsertFormData(FormInsertModel model)
        {
            List<InsertValidationModel> validation = new List<InsertValidationModel>();
            IFormDataCreator creator = DataService.CreateForm();

            SetPublicIdentifierStatus spi = creator.PublicIdentifier(model.FormPublicIdentifier);
            if (spi == SetPublicIdentifierStatus.NoSuchFormDefinition)
                validation.Add(new InsertValidationModel(null, "NoSuchFormDefinition"));

            if (!String.IsNullOrEmpty(model.ParentPublicIdentifier))
            {
                SetParentDataStatus spd = creator.Parent(model.ParentPublicIdentifier);
                switch (spd)
                {
                    case SetParentDataStatus.NoSuchFormData:
                        validation.Add(new InsertValidationModel(null, "NoSuchFormData"));
                        break;
                    case SetParentDataStatus.NoSuchFormDefinition:
                        validation.Add(new InsertValidationModel(null, "NoSuchFormDefinition"));
                        break;
                }
            }

            creator.Tag(model.FormTag);

            FormDefinition form = FormService.Get(model.FormPublicIdentifier);
            foreach (FieldInsertModel field in model.Fields)
            {
                FieldDefinition def = form.Fields.FirstOrDefault(f => f.PublicIdentifier == field.PublicIdentifier);
                AddFieldStatus afs;

                if (def != null && def.FieldType == FieldType.FileField)
                {
                    HttpPostedFileBase file = files[field.Value];
                    if (file != null)
                    {
                        MemoryStream stream = new MemoryStream();
                        file.InputStream.CopyTo(stream);
                        afs = creator.AddField(field.PublicIdentifier, file.FileName, file.ContentType, stream.ToArray());
                        files.Remove(field.Value);
                    }
                    else
                    {
                        afs = AddFieldStatus.IncorrectValue;
                    }
                }
                else
                {
                    afs = creator.AddFieldConvert(field.PublicIdentifier, field.Value);
                }

                switch (afs)
                {
                    case AddFieldStatus.NoSuchFormDefinition:
                        validation.Add(new InsertValidationModel(field.PublicIdentifier, "NoSuchFormDefinition"));
                        break;
                    case AddFieldStatus.NoSuchFieldDefinition:
                        validation.Add(new InsertValidationModel(field.PublicIdentifier, "NoSuchFieldDefinition"));
                        break;
                    case AddFieldStatus.IncorrectFieldType:
                        validation.Add(new InsertValidationModel(field.PublicIdentifier, "IncorrectFieldType"));
                        break;
                    case AddFieldStatus.IncorrectValue:
                        validation.Add(new InsertValidationModel(field.PublicIdentifier, "IncorrectValue"));
                        break;
                }
            }

            //TODO: Handle reference field!

            if (validation.Count == 0)
            {
                CreateFormDataStatus status = creator.Save();
                switch (status)
                {
                    case CreateFormDataStatus.InvalidCreator:
                        validation.Add(new InsertValidationModel(null, "InvalidCreator"));
                        break;
                }
            }

            if (validation.Count > 0)
            {
                //Response.StatusCode = 406;
                return JsonP(new
                {
                    Errors = validation
                });
            }

            DataService = DependencyResolver.Current.GetService<IFormDataService>(); //TODO: Never mind! 'Hack' for creating new DataContext
            return GetFormData(model.FormPublicIdentifier, 1, 0);
        }

        [Url("ws/file-{fieldID}")]
        public ActionResult File(int fieldID)
        {
            FileFieldData data = DataService.GetFileData(fieldID);
            byte[] fileData = FileStorage.GetData(data.LocalFilename);
            if(fileData == null)
                return new HttpStatusCodeResult(404);

            return File(fileData, data.MimeType, data.Filename);
        }

        [HttpGet]
        [Url("ws/{formPublicIdentifier}/inquiry-insert")]
        public ActionResult InsertInquiryData(string formPublicIdentifier, string fieldPublicIdentifier)
        {
            List<InsertValidationModel> validation = new List<InsertValidationModel>();
            IInquiryDataCreator creator = DataService.CreateInquiry();

            SetPublicIdentifierStatus spi = creator.PublicIdentifier(formPublicIdentifier);
            if (spi == SetPublicIdentifierStatus.NoSuchFormDefinition)
                validation.Add(new InsertValidationModel(null, "NoSuchFormDefinition"));

            AddInquiryAnswerStatus aias = creator.AddAnswer(fieldPublicIdentifier);
            switch (aias)
            {
                case AddInquiryAnswerStatus.NoSuchFormDefinition:
                    validation.Add(new InsertValidationModel(fieldPublicIdentifier, "NoSuchFormDefinition"));
                    break;
                case AddInquiryAnswerStatus.NoSuchFieldDefinition:
                    validation.Add(new InsertValidationModel(fieldPublicIdentifier, "NoSuchFieldDefinition"));
                    break;
            }

            CreateFormDataStatus status = creator.Save();
            switch (status)
            {
                case CreateFormDataStatus.InvalidCreator:
                    validation.Add(new InsertValidationModel(null, "InvalidCreator"));
                    break;
            }

            if (validation.Count > 0)
                return JsonP(validation);

            //TODO: Should return something! (Inserted data?)
            return new EmptyResult();
        }
    }
}
