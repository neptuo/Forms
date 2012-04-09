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
    public class FormDefinitionController : BaseController
    {
        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Dependency]
        public IProjectService ProjectService { get; set; }

        public ActionResult Create()
        {
            return View("Edit", new EditFormDefinitionModel
            {
                Projects = ProjectService.GetList().Select(p => new LightProject
                {
                    ID = p.ID,
                    Name = p.Name
                })
            });
        }

        public ActionResult Edit(int id)
        {
            FormDefinition form = FormService.Get(id);
            if (form == null)
            {
                ShowMessage((L)"No such form definition!", HtmlMessageType.Warning);
                return RedirectToAction("index", "project");
            }

            return View(new EditFormDefinitionModel
            {
                ID = form.ID,
                Name = form.Name,
                FormType = form.FormType,
                PublicContent = form.PublicContent,
                ProjectID = form.ProjectID
            });
        }

        [HttpPost]
        public ActionResult Edit(EditFormDefinitionModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsNew())
                {
                    CreateFormDefinitionStatus status = FormService.CreateForm(model.Name, model.FormType, model.PublicContent, model.ProjectID);
                    switch (status)
                    {
                        case CreateFormDefinitionStatus.Created:
                            ShowMessage(String.Format((L)"Form '{0}' created.", model.Name));
                            return RedirectToAction("forms", "project", new { id = model.ProjectID });
                        case CreateFormDefinitionStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid name!");
                            break;
                        case CreateFormDefinitionStatus.InvalidFormType:
                            ModelState.AddModelError("FormType", (L)"Invalid form type!");
                            break;
                        case CreateFormDefinitionStatus.NoSuchProject:
                            ShowMessage((L)"No such project!", HtmlMessageType.Warning);
                            return RedirectToAction("index", "project");
                    }
                }
                else
                {
                    UpdateFormDefinitionStatus status = FormService.UpdateForm(model.ID, model.Name, model.PublicContent);
                    switch (status)
                    {
                        case UpdateFormDefinitionStatus.Updated:
                            ShowMessage(String.Format((L)"Form '{0}' updated.", model.Name));
                            return RedirectToAction("forms", "project", new { id = model.ProjectID });
                        case UpdateFormDefinitionStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid name!");
                            break;
                        case UpdateFormDefinitionStatus.NoSuchFormDefinition:
                            ShowMessage((L)"No such form definition!", HtmlMessageType.Warning);
                            return RedirectToAction("forms", "project", new { id = model.ProjectID });
                    }
                }
            }

            return View(model);
        }

        public ActionResult Fields(int id)
        {
            return View(FormService.GetFields(id).Select(f => new ListFieldDefinitionModel
            {
                ID = f.ID,
                Name = f.Name,
                Required = f.Required,
                FieldType = f.FieldType
            }));
        }
    }
}
