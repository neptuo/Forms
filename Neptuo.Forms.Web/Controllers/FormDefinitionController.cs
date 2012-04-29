using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using RiaLibrary.Web;
using Neptuo.Web.Mvc.Html;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models;

namespace Neptuo.Forms.Web.Controllers
{
    [AuthorizeUser]
    public class FormDefinitionController : BaseController
    {
        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Dependency]
        public IProjectService ProjectService { get; set; }

        [Url("user/forms")]
        public ActionResult Index()
        {
            Project project = ProjectService.GetList().OrderBy(p => p.ID).FirstOrDefault();
            if (project != null)
                return RedirectToAction("forms", "project", new { projectID = project.ID });

            ShowMessage((L)"You don't have project, create one at first.", HtmlMessageType.Warning);
            return RedirectToAction("index", "project");
        }

        [Url("user/form/create")]
        public ActionResult Create(int projectID)
        {
            return View("Edit", new EditFormDefinitionModel
            {
                ProjectID = projectID,
                Projects = ProjectService.GetList().Select(p => new LightProject
                {
                    ID = p.ID,
                    Name = p.Name
                })
            });
        }

        [Url("user/form-{formDefinitionID}/edit")]
        public ActionResult Edit(int formDefinitionID)
        {
            FormDefinition form = FormService.Get(formDefinitionID);
            if (form == null)
            {
                ShowMessage((L)"No such form definition!", HtmlMessageType.Warning);
                return RedirectToAction("index", "project");
            }

            return View(new EditFormDefinitionModel
            {
                FormDefinitionID = form.ID,
                Name = form.Name,
                FormType = form.FormType,
                PublicContent = form.PublicContent,
                ProjectID = form.ProjectID
            });
        }

        [HttpPost]
        [Url("user/form-{formDefinitionID}/edit")]
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
                            return RedirectToAction("forms", "project", new { projectID = model.ProjectID });
                        case CreateFormDefinitionStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid name!");
                            break;
                        case CreateFormDefinitionStatus.InvalidFormType:
                            ModelState.AddModelError("FormType", (L)"Invalid form type!");
                            break;
                        case CreateFormDefinitionStatus.NoSuchProject:
                            ShowMessage((L)"No such project!", HtmlMessageType.Warning);
                            return RedirectToAction("index", "project");
                        case CreateFormDefinitionStatus.FormCountExceeded:
                            ShowMessage((L)"Maximum forms in project count exceeded!", HtmlMessageType.Warning);
                            return RedirectToAction("forms", "project", new { projectID = model.ProjectID });
                    }
                }
                else
                {
                    UpdateFormDefinitionStatus status = FormService.UpdateForm(model.FormDefinitionID, model.Name, model.PublicContent);
                    switch (status)
                    {
                        case UpdateFormDefinitionStatus.Updated:
                            ShowMessage(String.Format((L)"Form '{0}' updated.", model.Name));
                            return RedirectToAction("forms", "project", new { projectID = model.ProjectID });
                        case UpdateFormDefinitionStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid name!");
                            break;
                        case UpdateFormDefinitionStatus.NoSuchFormDefinition:
                            ShowMessage((L)"No such form definition!", HtmlMessageType.Warning);
                            return RedirectToAction("forms", "project", new { projectID = model.ProjectID });
                    }
                }
            }

            return View(model);
        }

        [Url("user/form-{formDefinitionID}/fields")]
        public ActionResult Fields(int formDefinitionID)
        {
            return View(new ListFieldDefinitionModel
            {
                ProjectID = FormService.Get(formDefinitionID).ProjectID,
                Fields = FormService.GetFields(formDefinitionID).Select(f => new ListItemFieldDefinitionModel
                {
                    FieldDefinitionID = f.ID,
                    Name = f.Name,
                    PublicIdentifier = f.PublicIdentifier,
                    Required = f.Required,
                    FieldType = f.FieldType
                })
            });
        }
    }
}
