using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Records all interactions with system.
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// Logs error in application.
        /// </summary>
        /// <param name="error">Thrown exception.</param>
        void ErrorThrown(Exception error);

        /// <summary>
        /// Logs successful user login.
        /// </summary>
        /// <param name="username">Current username.</param>
        void UserLoggedIn(string username);

        /// <summary>
        /// Logs successful user logout.
        /// </summary>
        /// <param name="username">Logged out user.</param>
        void UserLoggedOut(string username);

        /// <summary>
        /// Logs unsuccessful user login attempt.
        /// </summary>
        /// <param name="username">Username.</param>
        void UserLoginFailure(string username);

        /// <summary>
        /// Logs registration of new user.
        /// </summary>
        /// <param name="username">New user username.</param>
        void UserRegistered(string username);


        /// <summary>
        /// Logs new project creation.
        /// </summary>
        /// <param name="projectID">New project ID.</param>
        void ProjectCreated(int projectID);

        /// <summary>
        /// Logs project update.
        /// </summary>
        /// <param name="projectID">Updated project ID.</param>
        void ProjectUpdated(int projectID);


        /// <summary>
        /// Logs new form creation.
        /// </summary>
        /// <param name="formID">New form ID.</param>
        void FormDefinitionCreated(int formID);

        /// <summary>
        /// Logs form update.
        /// </summary>
        /// <param name="formID">Update form ID.</param>
        void FormDefinitionUpdated(int formID);


        /// <summary>
        /// Logs new field creation.
        /// </summary>
        /// <param name="fieldID">New field ID.</param>
        void FieldDefinitionCreated(int fieldID);

        /// <summary>
        /// Logs field update.
        /// </summary>
        /// <param name="fieldID">Updated field ID.</param>
        void FieldDefinitionUpdated(int fieldID);


        /// <summary>
        /// Logs invitation creation.
        /// </summary>
        /// <param name="invitationID">New project invitation ID.</param>
        void ProjectInvitationCreated(int invitationID);

        /// <summary>
        /// Logs invitation acception.
        /// </summary>
        /// <param name="invitationID">Accepted project invitation ID.</param>
        void ProjectInvitationAccepted(int invitationID);

        /// <summary>
        /// Logs invitation declination.
        /// </summary>
        /// <param name="invitationID">Declined project invitation ID.</param>
        void ProjectInvitationDeclined(int invitationID);

        /// <summary>
        /// Logs invitation deletion.
        /// </summary>
        /// <param name="invitationID">Deleted project invitation ID.</param>
        void ProjectInvitationDeleted(int invitationID);
    }
}
