using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// TODO: Log more meaningfull informations (not only ID).
    /// </summary>
    public class ActivityService : IActivityService
    {
        [Dependency]
        public ILogger Logger { get; set; }

        public void ErrorThrown(Exception error)
        {
            Logger.Log(error.Message);
        }

        public void UserLoggedIn(string username)
        {
            Logger.Log("User logged in. Username={0}", username);
        }

        public void UserLoggedOut(string username)
        {
            Logger.Log("User logged out. Username={0}", username);
        }

        public void UserLoginFailure(string username)
        {
            Logger.Log("User login failure. Username={0}", username);
        }

        public void UserRegistered(string username)
        {
            Logger.Log("New user registered. Username={0}", username);
        }

        public void ProjectCreated(int projectID)
        {
            Logger.Log("Project created. ID={0}", projectID);
        }

        public void ProjectUpdated(int projectID)
        {
            Logger.Log("Project updated. ID={0}", projectID);
        }

        public void FormDefinitionCreated(int formID)
        {
            Logger.Log("Form definition created. ID={0}", formID);
        }

        public void FormDefinitionUpdated(int formID)
        {
            Logger.Log("Form definition updated. ID={0}", formID);
        }

        public void FieldDefinitionCreated(int fieldID)
        {
            Logger.Log("Field definition created. ID={0}", fieldID);
        }

        public void FieldDefinitionUpdated(int fieldID)
        {
            Logger.Log("Field definition updated. ID={0}", fieldID);
        }

        public void ProjectInvitationCreated(int invitationID)
        {
            Logger.Log("Project invitation created. ID={0}", invitationID);
        }

        public void ProjectInvitationAccepted(int invitationID)
        {
            Logger.Log("Project invitation updated. ID={0}", invitationID);
        }

        public void ProjectInvitationDeclined(int invitationID)
        {
            Logger.Log("Project invitation declined. ID={0}", invitationID);
        }

        public void ProjectInvitationDeleted(int invitationID)
        {
            Logger.Log("Project invitation deleted. ID={0}", invitationID);
        }
    }
}
