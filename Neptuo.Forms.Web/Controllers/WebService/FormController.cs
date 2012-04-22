using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using RiaLibrary.Web;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models.WebService;

namespace Neptuo.Forms.Web.Controllers.WebService
{
    public class FormController : BaseController
    {
        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Dependency]
        public IFormDataService DataService { get; set; }

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
                    TargetFormPublicIdentifier = f.ReferenceFormID != null ? f.FormDefinition.PublicIdentifier : null,
                    TargetFieldPublicIdentifier = f.ReferenceDisplayFieldID != null ? f.ReferenceDisplayField.PublicIdentifier : null
                })
            });
        }

        [Url("ws/{formPublicIdentifier}/data")]
        public ActionResult GetFormData(string formPublicIdentifier, int pageSize = 20, int pageIndex = 0)
        {
            FormDefinition form = FormService.Get(formPublicIdentifier);
            if (form == null)
                return new HttpStatusCodeResult(404);

            IEnumerable<FormData> formData = DataService.GetList(form.ID).OrderByDescending(d => d.Created).Skip(pageSize * pageIndex).Take(pageSize).ToArray();

            return JsonP(formData.Select(d => new FormListDataModel {
                Created = d.Created,
                Fields = d.Fields.Select(f => new FieldListDataModel {
                    PublicIdentifier = f.FieldDefinition.PublicIdentifier,
                    Name = f.FieldDefinition.Name,
                    Value = f.GetDisplayValue()
                })
            }));
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

            creator.Tag(model.FormTag);

            foreach (FieldInsertModel field in model.Fields)
            {
                AddFieldStatus afs = creator.AddFieldConvert(field.PublicIdentifier, field.Value);
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
            //TODO: What about file field???

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
