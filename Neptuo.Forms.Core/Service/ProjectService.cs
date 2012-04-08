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

        public IQueryable<Project> GetList(int userID)
        {
            return Repository.Where(p => p.OwnerUserID == userID).OrderBy(p => p.ID);
        }

        public Project Get(int id, int userID)
        {
            return Repository.FirstOrDefault(p => p.ID == id && p.OwnerUserID == userID);
        }

        public CreateProjectStatus CreateProject(string name, string description, int userID)
        {
            if (String.IsNullOrEmpty(name))
                return CreateProjectStatus.InvalidName;

            Project project = new Project
            {
                Name = name,
                Description = description,
                OwnerUserID = userID,
                Created = DateTime.Now
            };
            Repository.Insert(project);
            return CreateProjectStatus.Created;
        }

        public UpdateProjectStatus UpdateProject(int id, string name, string description, int userID)
        {
            if (String.IsNullOrEmpty(name))
                return UpdateProjectStatus.InvalidName;

            Project project = Repository.FirstOrDefault(p => p.ID == id && p.OwnerUserID == userID);
            if (project == null)
                return UpdateProjectStatus.NoSuchProject;

            project.Name = name;
            project.Description = description;
            Repository.Update(project);
            return UpdateProjectStatus.Updated;
        }
    }
}
