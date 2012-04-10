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
    public class ProjectController : BaseController
    {
        [Dependency]
        public IProjectService ProjectService { get; set; }

        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Url("user/projects")]
        public ActionResult Index()
        {
            return View(ProjectService.GetList().Select(p => new ListProjectModel
            {
                ProjectID = p.ID,
                Name = p.Name,
                Created = p.Created
            }));
        }

        [Url("user/project/create")]
        public ActionResult Create()
        {
            return View("Edit", new EditProjectModel());
        }

        [Url("user/project-{projectID}/edit")]
        public ActionResult Edit(int projectID)
        {
            Project project = ProjectService.Get(projectID);
            if (project == null)
            {
                ShowMessage((L)"No such project!", HtmlMessageType.Error);
                return RedirectToAction("index");
            }

            return View(new EditProjectModel
            {
                ProjectID = project.ID,
                Name = project.Name,
                Description = project.Description
            });
        }

        [HttpPost]
        [Url("user/project-{projectID}/edit")]
        public ActionResult Edit(EditProjectModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsNew())
                {
                    CreateProjectStatus status = ProjectService.CreateProject(model.Name, model.Description);
                    switch (status)
                    {
                        case CreateProjectStatus.Created:
                            ShowMessage(String.Format((L)"Project '{0}' created.", model.Name));
                            return RedirectToAction("index");
                        case CreateProjectStatus.ProjectCountExceeded:
                            ShowMessage((L)"Maximum project count exceeded!", HtmlMessageType.Warning);
                            return RedirectToAction("index");
                        case CreateProjectStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid project name!");
                            break;
                    }
                }
                else
                {
                    UpdateProjectStatus status = ProjectService.UpdateProject(model.ProjectID, model.Name, model.Description);
                    switch (status)
                    {
                        case UpdateProjectStatus.Updated:
                            ShowMessage(String.Format((L)"Project '{0}' updated.", model.Name));
                            return RedirectToAction("index");
                        case UpdateProjectStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid project name!");
                            break;
                        case UpdateProjectStatus.NoSuchProject:
                            ShowMessage((L)"No such project!", HtmlMessageType.Warning);
                            return RedirectToAction("index");
                    }
                }
            }

            return View(model);
        }

        [Url("user/project-{projectID}/forms")]
        public ActionResult Forms(int projectID)
        {
            return View(new ListFormDefinitionModel
            {
                Forms = FormService.GetList(projectID).Select(f => new ListItemFormDefinitionModel
                {
                    FormDefinitionID = f.ID,
                    Name = f.Name,
                    PublicIdentifier = f.PublicIdentifier,
                    PublicContent = f.PublicContent,
                    Created = f.Created,
                    FormType = f.FormType
                }),
                Projects = ProjectService.GetList().Select(p => new LightProject
                {
                    ID = p.ID,
                    Name = p.Name
                }),
                CurrentProjectID = projectID
            });
        }

    }
}
