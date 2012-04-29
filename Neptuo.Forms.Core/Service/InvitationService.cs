using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;

namespace Neptuo.Forms.Core.Service
{
    public class InvitationService : IInvitationService
    {
        [Dependency]
        public IRepository<ProjectInvitation> ProjectInvitations { get; set; }

        [Dependency]
        public IProjectService ProjectService { get; set; }

        [Dependency]
        public IUserService UserService { get; set; }

        [Dependency]
        public UserContext UserContext { get; set; }

        [Dependency]
        public IActivityService ActivityService { get; set; }

        public CreateInvitationStatus InviteToProject(int projectID, string accountPublicIdentifier, int type)
        {
            Project project = ProjectService.Get(projectID);
            if (project == null || !ProjectService.IsUserOwner(projectID))
                return CreateInvitationStatus.NotOwner;

            UserAccount account = UserService.GetByIdentifier(accountPublicIdentifier);
            if (account == null)
                return CreateInvitationStatus.NoSuchUser;

            ProjectInvitation other = ProjectInvitations.FirstOrDefault(pi => pi.TargetProjectID == projectID && pi.TargetUserID == account.ID);
            if (other != null)
            {
                if (other.Type == type)
                    return CreateInvitationStatus.AlreadyExists;

                other.Type = type;
                ProjectInvitations.Update(other);
                return CreateInvitationStatus.UpdatedExisting;
            }

            ProjectInvitation invitation = new ProjectInvitation
            {
                Created = DateTime.Now,
                TargetProjectID = projectID,
                TargetUserID = account.ID,
                Type = type
            };
            ProjectInvitations.Insert(invitation);
            ActivityService.ProjectInvitationCreated(invitation.ID);
            return CreateInvitationStatus.Created;
        }

        public AcceptInvitationStatus AcceptProjectInvitation(int invitationID)
        {
            ProjectInvitation invitation = ProjectInvitations.Get(invitationID);
            if (invitation == null)
                return AcceptInvitationStatus.NoSuchInvitation;

            if (invitation.TargetUserID != UserContext.AccountID)
                return AcceptInvitationStatus.NotTarget;

            //TODO: Assign to project

            ActivityService.ProjectInvitationAccepted(invitation.ID);
            ProjectInvitations.Delete(invitation);
            return AcceptInvitationStatus.Accepted;
        }

        public DeclineInvitationStatus DeclineProjectInvitation(int invitationID)
        {
            ProjectInvitation invitation = ProjectInvitations.Get(invitationID);
            if (invitation == null)
                return DeclineInvitationStatus.NoSuchInvitation;

            if (invitation.TargetUserID != UserContext.AccountID)
                return DeclineInvitationStatus.NotTarget;

            ActivityService.ProjectInvitationAccepted(invitation.ID);
            ProjectInvitations.Delete(invitation);
            return DeclineInvitationStatus.Declined;
        }

        public DeleteInvitationStatus DeleteProjectInvitation(int invitationID)
        {
            ProjectInvitation invitation = ProjectInvitations.Get(invitationID);
            if (invitation == null)
                return DeleteInvitationStatus.NoSuchInvitation;

            if (invitation.TargetProject.OwnerUserID != UserContext.AccountID)
                return DeleteInvitationStatus.NotOwner;

            ActivityService.ProjectInvitationDeleted(invitation.ID);
            ProjectInvitations.Delete(invitation);
            return DeleteInvitationStatus.Deleted;
        }

        public IQueryable<ProjectInvitation> GetProjectInvitations()
        {
            return ProjectInvitations.Where(pi => pi.TargetUserID == UserContext.AccountID);
        }

        public IQueryable<ProjectInvitation> GetCreatedProjectInvitations(int projectID)
        {
            if (!ProjectService.IsUserOwner(UserContext.AccountID))
                return new List<ProjectInvitation>().AsQueryable();

            return ProjectInvitations.Where(pi => pi.TargetProjectID == projectID);
        }
    }
}
