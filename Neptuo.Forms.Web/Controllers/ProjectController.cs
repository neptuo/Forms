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
    [Authorize]
    public class ProjectController : BaseController
    {
        [Dependency]
        public IProjectService ProjectService { get; set; }

        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        public ActionResult Index()
        {
            return View(ProjectService.GetList().Select(p => new ListProjectModel
            {
                ID = p.ID,
                Name = p.Name,
                Created = p.Created
            }));
        }

        public ActionResult Create()
        {
            return View("Edit", new EditProjectModel());
        }

        public ActionResult Edit(int id)
        {
            Project project = ProjectService.Get(id);
            if (project == null)
            {
                ShowMessage((L)"No such project!", HtmlMessageType.Error);
                return RedirectToAction("index");
            }

            return View(new EditProjectModel
            {
                ID = project.ID,
                Name = project.Name,
                Description = project.Description
            });
        }

        [HttpPost]
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
                        case CreateProjectStatus.InvalidName:
                            ModelState.AddModelError("Name", (L)"Invalid project name!");
                            break;
                    }
                }
                else
                {
                    UpdateProjectStatus status = ProjectService.UpdateProject(model.ID, model.Name, model.Description);
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

        public ActionResult Forms(int id)
        {
            return View(FormService.GetList(id).Select(f => new ListFormDefinitionModel
            {
                ID = f.ID,
                Name = f.Name,
                PublicIdentifier = f.PublicIdentifier,
                PublicContent = f.PublicContent,
                Created = f.Created,
                FormType = f.FormType
            }));
        }

    }
}
