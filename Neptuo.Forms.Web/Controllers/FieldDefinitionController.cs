using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Neptuo.Web.Mvc.Html;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models;

namespace Neptuo.Forms.Web.Controllers
{
    public class FieldDefinitionController : BaseController
    {
        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        public ActionResult Create(int formDefinitionID)
        {
            FormDefinition form = FormService.Get(formDefinitionID);
            if(form == null) {
                ShowMessage((L)"No such form!", HtmlMessageType.Warning);
                return RedirectToAction("index", "project");
            }

            return View("edit", new EditFieldDefinitionModel
            {
                FormDefinitionID = formDefinitionID,
                FormType = form.FormType,
                FieldType = form.FormType == FormType.Form ? FieldType.StringField : FieldType.BoolField
            });
        }

        public ActionResult Edit(int id)
        {
            FieldDefinition field = FormService.GetField(id);
            if (field == null)
            {
                ShowMessage((L)"No such field!", HtmlMessageType.Warning);
                return RedirectToAction("index", "project");
            }

            return View(new EditFieldDefinitionModel
            {
                ID = field.ID,
                Name = field.Name,
                Required = field.Required,
                FieldType = field.FieldType,
                FormDefinitionID = field.FormDefinitionID,
                FormType = field.FormDefinition.FormType
            });
        }

        [HttpPost]
        public ActionResult Edit(EditFieldDefinitionModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsNew())
                {
                    CreateFieldDefinitionStatus status = FormService.AddField(model.FormDefinitionID, model.Name, model.FieldType, model.Required);
                    switch (status)
                    {
                        case CreateFieldDefinitionStatus.Created:
                            ShowMessage(String.Format((L)"Form field '{0}' created.", model.Name));
                            return RedirectToAction("fields", "formdefinition", new { id = model.FormDefinitionID });
                        case CreateFieldDefinitionStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid field name!");
                            break;
                        case CreateFieldDefinitionStatus.InvalidFieldType:
                            ModelState.AddModelError("FieldType", (L)"Invalid field type!");
                            break;
                        case CreateFieldDefinitionStatus.NoSuchFormDefinition:
                            ShowMessage((L)"No such form definition!", HtmlMessageType.Warning);
                            return RedirectToAction("index", "project");
                    }
                }
                else
                {
                    UpdateFieldDefinitionStatus status = FormService.UpdateField(model.ID, model.Name, model.Required);
                    switch (status)
                    {
                        case UpdateFieldDefinitionStatus.Updated:
                            ShowMessage(String.Format((L)"Form field '{0}' updated.", model.Name));
                            return RedirectToAction("fields", "formdefinition", new { id = model.FormDefinitionID });
                        case UpdateFieldDefinitionStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid field name!");
                            break;
                        case UpdateFieldDefinitionStatus.NoSuchFieldDefinition:
                            ShowMessage((L)"No such form definition!", HtmlMessageType.Warning);
                            return RedirectToAction("index", "project");
                    }
                }
            }
            return View(model);
        }

    }
}
