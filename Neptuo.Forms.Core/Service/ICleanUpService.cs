using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Service for deleting form items (projects, definitions, data).
    /// </summary>
    public interface ICleanUpService
    {
        /// <summary>
        /// Tries to delete project by <paramref name="projectID"/>.
        /// Also deletes all form definitions.
        /// Also deletes all data.
        /// </summary>
        /// <param name="projectID">ID of project to delete.</param>
        /// <returns>Deletion status, <see cref="DeleteProjectStatus"/>.</returns>
        DeleteProjectStatus DeleteProject(int projectID);

        /// <summary>
        /// Tries to delete form definition by <paramref name="formDefinitionID"/>.
        /// Also deletes all data.
        /// </summary>
        /// <param name="projectID">ID of form definition to delete.</param>
        /// <returns>Deletion status, <see cref="DeleteFormDefinitionStatus"/>.</returns>
        DeleteFormDefinitionStatus DeleteFormDefinition(int formDefinitionID);

        /// <summary>
        /// Tries to delete form data by <paramref name="formDataID"/>.
        /// </summary>
        /// <param name="projectID">ID of form data to delete.</param>
        /// <returns>Deletion status, <see cref="DeleteFormDataStatus"/>.</returns>
        DeleteFormDataStatus DeleteFormData(int formDataID);
    }

    /// <summary>
    /// Enumeration of possible states that can occur while deleting project.
    /// </summary>
    public enum DeleteProjectStatus
    {
        /// <summary>
        /// Project deleted.
        /// </summary>
        Deleted, 
        
        /// <summary>
        /// Error state, current user doesn't have required pemissions.
        /// </summary>
        PermissionDenied, 
        
        /// <summary>
        /// Error state, no such project.
        /// </summary>
        NoSuchProject
    }

    /// <summary>
    /// Enumeration of possible states that can occur while deleting form definition.
    /// </summary>
    public enum DeleteFormDefinitionStatus
    {
        /// <summary>
        /// Form definition deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// Error state, current user doesn't have required pemissions.
        /// </summary>
        PermissionDenied,

        /// <summary>
        /// Error state, no such form definition.
        /// </summary>
        NoSuchFormDefinition
    }

    /// <summary>
    /// Enumeration of possible states that can occur while deleting form data.
    /// </summary>
    public enum DeleteFormDataStatus
    {
        /// <summary>
        /// Form data deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// Error state, current user doesn't have required pemissions.
        /// </summary>
        PermissionDenied,

        /// <summary>
        /// Error state, no such form data.
        /// </summary>
        NoSuchFormData
    }
}
