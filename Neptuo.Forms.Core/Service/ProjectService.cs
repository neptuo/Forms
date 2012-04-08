using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;

namespace Neptuo.Forms.Core.Service
{
    public class ProjectService : IProjectService
    {
        [Dependency]
        public IRepository<Project> Repository { get; set; }

        [Dependency]
        public UserContext UserContext { get; set; }

        public IQueryable<Project> GetList()
        {
            return Repository.Where(p => p.OwnerUserID == UserContext.AccountID).OrderBy(p => p.ID);
        }

        public Project Get(int id)
        {
            return Repository.FirstOrDefault(p => p.ID == id && p.OwnerUserID == UserContext.AccountID);
        }

        public CreateProjectStatus CreateProject(string name, string description)
        {
            if (String.IsNullOrEmpty(name))
                return CreateProjectStatus.InvalidName;

            Project project = new Project
            {
                Name = name,
                Description = description,
                OwnerUserID = UserContext.AccountID,
                Created = DateTime.Now
            };
            Repository.Insert(project);
            return CreateProjectStatus.Created;
        }

        public UpdateProjectStatus UpdateProject(int id, string name, string description)
        {
            if (String.IsNullOrEmpty(name))
                return UpdateProjectStatus.InvalidName;

            Project project = Get(id);
            if (project == null)
                return UpdateProjectStatus.NoSuchProject;

            project.Name = name;
            project.Description = description;
            Repository.Update(project);
            return UpdateProjectStatus.Updated;
        }

        public bool CanUserRead(int id)
        {
            Project project = Get(id);
            return project != null;
        }

        public bool CanUserManage(int id)
        {
            Project project = Get(id);
            return project != null;
        }

        public bool IsUserOwner(int id)
        {
            Project project = Get(id);
            return project != null;
        }
    }
}
