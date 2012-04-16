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

        [Url("ws/{formPublicIdentifier}/definition")]
        public object GetDefinition(string formPublicIdentifier)
        {
            FormDefinition form = FormService.Get(formPublicIdentifier);
            if (form == null)
                return new HttpStatusCodeResult(404);

            return Json(new FormDefinitionModel
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
            }, JsonRequestBehavior.AllowGet);
        }

        [Url("ws/{formPublicIdentifier}/data")]
        public ActionResult GetData(string formPublicIdentifier)
        {
            return View();
        }

        [Url("ws/{formPublicIdentifier}/insert")]
        public ActionResult InsertData()
        {
            return View();
        }
    }
}
