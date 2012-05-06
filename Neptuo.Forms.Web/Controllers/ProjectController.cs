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
        public IInvitationService InvitationService { get; set; }

        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Url("user/projects")]
        public ActionResult Index()
        {
            return View(ProjectService.GetList().Select(p => new ListProjectModel
            {
                ProjectID = p.ID,
                Name = p.Name,
                Created = p.Created,
                IsOwner = p.OwnerUserID == UserContext.AccountID
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

        [Url("user/project-{projectID}/invitations")]
        public ActionResult Invitations(int projectID)
        {
            if (!ProjectService.IsUserOwner(projectID))
            {
                ShowMessage((L)"You are not project onwer! Only project owner can manage invitations.", HtmlMessageType.Warning);
                return RedirectToAction("index");
            }

            return View(new ListInvitationModel
            {
                Invitations = InvitationService.GetCreatedProjectInvitations(projectID).Select(pi => new ListItemInvitationModel
                {
                    ID = pi.ID,
                    ProjectName = pi.TargetProject.Name,
                    OwnerFullname = pi.TargetProject.Owner.Fullname,
                    OwnerPublicIdentifier = pi.TargetProject.Owner.PublicIdentifier,
                    TargetUserFullname = pi.TargetUser.Fullname,
                    TargetUserPublicIdentifier = pi.TargetUser.PublicIdentifier,
                    Created = pi.Created,
                    Type = pi.Type
                }),
                CreateModel = new CreateInvitationModel()
            });
        }

        [HttpPost]
        [Url("user/project-{projectID}/delete-invitation-{invitationID}")]
        public ActionResult DeleteInvitation(int projectID, int invitationID)
        {
            DeleteInvitationStatus status = InvitationService.DeleteProjectInvitation(invitationID);
            switch (status)
            {
                case DeleteInvitationStatus.Deleted:
                    ShowMessage((L)"Invitation deleted.");
                    return RedirectToAction("index");
                case DeleteInvitationStatus.NotOwner:
                    ShowMessage((L)"You are not project onwer! Only project owner can delete invitations.", HtmlMessageType.Warning);
                    return RedirectToAction("index");
                case DeleteInvitationStatus.NoSuchInvitation:
                    ShowMessage((L)"No such project invitation.", HtmlMessageType.Warning);
                    return RedirectToAction("Invitations", new { projectID = projectID });
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [Url("user/project-{projectID}/create-invitation")]
        public ActionResult CreateInvitation(int projectID, [Bind(Prefix="CreateModel")] CreateInvitationModel model)
        {
            if (ModelState.IsValid)
            {
                CreateInvitationStatus status = InvitationService.InviteToProject(projectID, model.TargetUserPublicIdentifier, model.Type);
                switch (status)
                {
                    case CreateInvitationStatus.Created:
                        ShowMessage((L)"Invitation created.");
                        return RedirectToAction("Invitations", new { projectID = projectID });
                    case CreateInvitationStatus.UpdatedExisting:
                        ShowMessage((L)"Existing invitation was updated.");
                        return RedirectToAction("Invitations", new { projectID = projectID });
                    case CreateInvitationStatus.NotOwner:
                        ShowMessage((L)"You're not owner! Only project owner can create invitations.", HtmlMessageType.Warning);
                        return RedirectToAction("index");
                    case CreateInvitationStatus.AlreadyExists:
                        ShowMessage((L)"Invitation already exists.", HtmlMessageType.Warning);
                        return RedirectToAction("Invitations", new { projectID = projectID });
                    case CreateInvitationStatus.NoSuchProject:
                        ShowMessage((L)"No such project.", HtmlMessageType.Warning);
                        return RedirectToAction("index");
                    case CreateInvitationStatus.NoSuchUser:
                        ShowMessage((L)"Target user doesn't exist.", HtmlMessageType.Warning);
                        return RedirectToAction("Invitations", new { projectID = projectID });
                }
            }

            return RedirectToAction("Invitations", new { projectID = projectID });
        }

        [ChildActionOnly]
        public ActionResult MyInvitations()
        {
            IQueryable<ProjectInvitation> invitations = InvitationService.GetProjectInvitations();
            if (invitations.Count() > 0)
            {
                return PartialView(invitations.Select(i => new MyInvitationModel
                {
                    ID = i.ID,
                    ProjectName = i.TargetProject.Name,
                    OwnerFullname = i.OwnerUser.Fullname,
                    Created = i.Created,
                    Type = i.Type
                }));
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AcceptInvitation(int invitationID)
        {
            AcceptInvitationStatus status = InvitationService.AcceptProjectInvitation(invitationID);
            switch (status)
            {
                case AcceptInvitationStatus.Accepted:
                    ShowMessage((L)"Invitation accepted.");
                    return RedirectToAction("Index", "FormDefinition");
                case AcceptInvitationStatus.NotTarget:
                    ShowMessage((L)"You are not the invited user!", HtmlMessageType.Warning);
                    return RedirectToAction("Index", "FormDefinition");
                case AcceptInvitationStatus.NoSuchInvitation:
                    ShowMessage((L)"Invitations doesn't exist!", HtmlMessageType.Warning);
                    return RedirectToAction("Index", "FormDefinition");
            }

            return RedirectToAction("Index", "FormDefinition");
        }

        [HttpPost]
        public ActionResult DeclineInvitation(int invitationID)
        {
            DeclineInvitationStatus status = InvitationService.DeclineProjectInvitation(invitationID);
            switch (status)
            {
                case DeclineInvitationStatus.Declined:
                    ShowMessage((L)"Invitation declined.");
                    return RedirectToAction("Index", "FormDefinition");
                case DeclineInvitationStatus.NotTarget:
                    ShowMessage((L)"You are not the invited user!", HtmlMessageType.Warning);
                    return RedirectToAction("Index", "FormDefinition");
                case DeclineInvitationStatus.NoSuchInvitation:
                    ShowMessage((L)"Invitations doesn't exist!", HtmlMessageType.Warning);
                    return RedirectToAction("Index", "FormDefinition");
            }

            return RedirectToAction("Index", "FormDefinition");
        }
    }
}
