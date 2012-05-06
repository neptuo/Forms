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
using Neptuo.Web.Mvc.Models;

namespace Neptuo.Forms.Web.Controllers
{
    [AuthorizeUser]
    public class FormDefinitionController : BaseController
    {
        private int pageSize = 10;

        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Dependency]
        public IProjectService ProjectService { get; set; }

        [Dependency]
        public IFormDataService DataService { get; set; }

        [Dependency]
        public ICleanUpService CleanUpService { get; set; }

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
                        case CreateFormDefinitionStatus.PermissionDenied:
                            ShowMessage((L)"You can't create form!", HtmlMessageType.Warning);
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
                        case UpdateFormDefinitionStatus.PermissionDenied:
                            ShowMessage((L)"You can't update this form!", HtmlMessageType.Warning);
                            return RedirectToAction("forms", "project", new { projectID = model.ProjectID });
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [Url("user/form-{formDefinitionID}/delete")]
        public ActionResult Delete(int formDefinitionID)
        {
            FormDefinition form = FormService.Get(formDefinitionID);
            DeleteFormDefinitionStatus status = CleanUpService.DeleteFormDefinition(formDefinitionID);
            switch (status)
            {
                case DeleteFormDefinitionStatus.Deleted:
                    ShowMessage((L)"Form definition deleted.");
                    return RedirectToAction("forms", "project", new { projectID = form.ProjectID });
                case DeleteFormDefinitionStatus.PermissionDenied:
                    ShowMessage((L)"Permission denied!", HtmlMessageType.Warning);
                    return RedirectToAction("forms", "project", new { projectID = form.ProjectID });
                case DeleteFormDefinitionStatus.NoSuchFormDefinition:
                    ShowMessage((L)"No such form definition.");
                    return RedirectToAction("index", "project");
            }
            return RedirectToAction("index", "project");
        }

        [Url("user/form-{formDefinitionID}/fields")]
        public ActionResult Fields(int formDefinitionID)
        {
            FormDefinition form = FormService.Get(formDefinitionID);
            return View(new ListFieldDefinitionModel
            {
                ProjectID = form.ProjectID,
                FormName = form.Name,
                FormType = form.FormType,
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

        [Url("user/form-{formDefinitionID}/data")]
        public ActionResult FormData(int formDefinitionID, int page = 1)
        {
            FormDefinition form = FormService.Get(formDefinitionID);
            return View(new ListFormDataModel
            {
                ProjectID = form.ProjectID,
                FormName = form.Name,
                Columns = FormService.GetFields(formDefinitionID).Select(f => new SimpleColumn
                {
                    ID = f.ID,
                    Name = f.Name
                }).ToList(),
                Items = PagingHelper.TakePage(DataService.GetList(formDefinitionID).Select(d => new ListItemFormDataModel
                {
                    ID = d.ID,
                    Created = d.Created,
                    Columns = d.Fields
                }), page, pageSize),
                PagingInfo = PagingHelper.CreateInfo(DataService.GetList(formDefinitionID), page, pageSize)
            });
        }
    }
}
