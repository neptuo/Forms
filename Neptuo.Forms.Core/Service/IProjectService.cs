using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Provides access to user projects.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Returns list of current user projects (at least can read there).
        /// </summary>
        /// <returns>List of current user projects.</returns>
        IQueryable<Project> GetList();

        /// <summary>
        /// Gets project by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>Project by <paramref name="id"/>.</returns>
        Project Get(int id);

        /// <summary>
        /// Creates new project.
        /// </summary>
        /// <param name="name">New project name.</param>
        /// <param name="description">New project description.</param>
        /// <returns>Creation status, <see cref="CreateProjectStatus"/>.</returns>
        CreateProjectStatus CreateProject(string name, string description);

        /// <summary>
        /// Updates existing project.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <param name="name">New name.</param>
        /// <param name="description">New description.</param>
        /// <returns>Update status, <see cref="UpdateProjectStatus"/>.</returns>
        UpdateProjectStatus UpdateProject(int id, string name, string description);

        /// <summary>
        /// Determines whether current user can <b>read</b> in project specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>If user can read in the project, then <code>true</code>. Otherwise <code>false</code>.</returns>
        bool CanUserRead(int id);

        /// <summary>
        /// Determines whether current user can <b>write</b> to project specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>If user can write to the project, then <code>true</code>. Otherwise <code>false</code>.</returns>
        bool CanUserManage(int id);

        /// <summary>
        /// Determines whether current user <b>owns</b> project specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>If user owns the project, then <code>true</code>. Otherwise <code>false</code>.</returns>
        bool IsUserOwner(int id);
    }

    /// <summary>
    /// Enumeration of states that can occur when creating new project.
    /// </summary>
    public enum CreateProjectStatus
    {
        /// <summary>
        /// Project created.
        /// </summary>
        Created, 
        
        /// <summary>
        /// Error state, invalid name was provided.
        /// </summary>
        InvalidName, 
        
        /// <summary>
        /// Error state, user has too many projects (<see cref="Validator.MaxUserProjects"/>).
        /// </summary>
        ProjectCountExceeded
    }

    /// <summary>
    /// Enumeration of statues that can occur when updating project.
    /// </summary>
    public enum UpdateProjectStatus
    {
        /// <summary>
        /// Project updated.
        /// </summary>
        Updated,

        /// <summary>
        /// Error state, invalid name was provided.
        /// </summary>
        InvalidName, 
        
        /// <summary>
        /// Error state, there was provided ID to non-existing project.
        /// </summary>
        NoSuchProject
    }
}
