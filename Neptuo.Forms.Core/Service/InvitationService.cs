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

        public CreateInvitationStatus InviteToProject(int projectID, string accountPublicIdentifier, int type)
        {
            Project project = ProjectService.Get(projectID);
            if (project == null || !ProjectService.IsUserOwner(projectID))
                return CreateInvitationStatus.NotOwner;

            UserAccount account = null;//UserService.Get()
            if (account == null)
                return CreateInvitationStatus.NoSuchUser;

            ProjectInvitation other = ProjectInvitations.FirstOrDefault(pi => pi.TargetProjectID == projectID && pi.TargetUserID == account.ID);
            if (other != null)
            {
                if (other.Type != type)
                {
                    other.Type = type;
                    ProjectInvitations.Update(other);
                    return CreateInvitationStatus.UpdatedExisting;
                }
                else
                {
                    return CreateInvitationStatus.AlreadyExists;
                }
            }

            ProjectInvitations.Insert(new ProjectInvitation
            {
                Created = DateTime.Now,
                TargetProjectID = projectID,
                TargetUserID = account.ID,
                Type = type
            });
            return CreateInvitationStatus.Created;
        }

        public AcceptInvitationStatus AcceptInvitation(int invitationID)
        {
            throw new NotImplementedException();
        }

        public DeclineInvitationStatus DeclineInvitation(int invitationID)
        {
            throw new NotImplementedException();
        }
    }
}
