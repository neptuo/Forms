using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Manager for invitations.
    /// </summary>
    public interface IInvitationService
    {
        /// <summary>
        /// Creates invitation to project.
        /// Current user must be project owner.
        /// </summary>
        /// <param name="projectID">Target project ID.</param>
        /// <param name="accountPublicIdentifier">Target account public identifier.</param>
        /// <param name="type">Type of role of target user in project, <see cref="ProjectInvitationType"/>.</param>
        /// <returns>Creation status, <see cref="CreateInvitationStatus"/>.</returns>
        CreateInvitationStatus InviteToProject(int projectID, string accountPublicIdentifier, int type);

        /// <summary>
        /// Tries to accept invitation.
        /// Current user must be invitation target.
        /// </summary>
        /// <param name="invitationID">ID of invitation.</param>
        /// <returns>Acception status, <see cref="AcceptInvitationStatus"/>.</returns>
        AcceptInvitationStatus AcceptProjectInvitation(int invitationID);

        /// <summary>
        /// Tries to decline invitation.
        /// Current user muset invitation target.
        /// </summary>
        /// <param name="invitationID">ID of invitation.</param>
        /// <returns>Declination status, <see cref="DeclineInvitationStatus"/>.</returns>
        DeclineInvitationStatus DeclineProjectInvitation(int invitationID);

        /// <summary>
        /// Tries to delete invitation (only for project owner).
        /// </summary>
        /// <param name="invitationID">ID of invitation.</param>
        /// <returns>Deletion status, <see cref="DeleteInvitationStatus"/>.</returns>
        DeleteInvitationStatus DeleteProjectInvitation(int invitationID);

        /// <summary>
        /// Returns invitations for current user.
        /// </summary>
        /// <returns>Pending invitations for current user.</returns>
        IQueryable<ProjectInvitation> GetProjectInvitations();

        /// <summary>
        /// Returns pending invitations for project (for project owner only).
        /// </summary>
        /// <param name="projectID">Project ID.</param>
        /// <returns>Pending invitations for project.</returns>
        IQueryable<ProjectInvitation> GetCreatedProjectInvitations(int projectID);
    }

    /// <summary>
    /// Enumeration of possible states that can occur while creating invitation.
    /// </summary>
    public enum CreateInvitationStatus
    {
        /// <summary>
        /// Invitation created.
        /// </summary>
        Created, 

        /// <summary>
        /// Type of currently existing invitation was updated.
        /// </summary>
        UpdatedExisting,
        
        /// <summary>
        /// Error state, current user isn't project owner.
        /// </summary>
        NotOwner, 
        
        /// <summary>
        /// Error state, same invitation already exists.
        /// </summary>
        AlreadyExists,
        
        /// <summary>
        /// Error state, no such project.
        /// </summary>
        NoSuchProject,

        /// <summary>
        /// Error state, no such target user.
        /// </summary>
        NoSuchUser
    }

    /// <summary>
    /// Enumeration of possible states that can occur while accepting invitation.
    /// </summary>
    public enum AcceptInvitationStatus
    {
        /// <summary>
        /// Invitation accepted.
        /// </summary>
        Accepted, 
        
        /// <summary>
        /// Error state, current user isn't invitation target.
        /// </summary>
        NotTarget, 
        
        /// <summary>
        /// Error state, no such invitation.
        /// </summary>
        NoSuchInvitation
    }

    /// <summary>
    /// Enumeration of possible states that can occur while declining invitation.
    /// </summary>
    public enum DeclineInvitationStatus
    {
        /// <summary>
        /// Invitation declined.
        /// </summary>
        Declined,

        /// <summary>
        /// Error state, current user isn't invitation target.
        /// </summary>
        NotTarget,

        /// <summary>
        /// Error state, no such invitation.
        /// </summary>
        NoSuchInvitation
    }

    /// <summary>
    /// Enumeration of possible states that can occur while declining invitation.
    /// </summary>
    public enum DeleteInvitationStatus
    {
        /// <summary>
        /// Invitation declined.
        /// </summary>
        Deleted,

        /// <summary>
        /// Error state, current user isn't project owner.
        /// </summary>
        NotOwner,

        /// <summary>
        /// Error state, no such invitation.
        /// </summary>
        NoSuchInvitation
    }
}
