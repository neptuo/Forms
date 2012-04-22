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
        public ActionResult GetData(string formPublicIdentifier)
        {
            return View();
        }

        [HttpGet]
        //[HttpPost]
        [Url("ws/{formPublicIdentifier}/insert")]
        public ActionResult InsertData(FormInsertModel model)
        {
            IFormDataCreator creator = DataService.Create();

            SetPublicIdentifierStatus spi = creator.PublicIdentifier(model.FormPublicIdentifier);
            if (spi == SetPublicIdentifierStatus.NoSuchFormDefinition)
                throw new Exception();

            creator.Tag(model.FormTag);

            foreach (FieldInsertModel field in model.Fields)
            {
                AddFieldStatus afs = creator.AddFieldConvert(field.PublicIndetifier, field.Value);
                switch (afs)
                {
                    case AddFieldStatus.NoSuchFormDefinition:
                        throw new Exception();
                    case AddFieldStatus.NoSuchFieldDefinition:
                        throw new Exception();
                    case AddFieldStatus.IncorrectFieldType:
                        throw new Exception();
                    case AddFieldStatus.IncorrectValue:
                        throw new Exception();
                }
            }
            creator.Save();


            return new EmptyResult();
        }
    }
}
