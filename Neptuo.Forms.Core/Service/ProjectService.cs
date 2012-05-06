using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;
using Neptuo.Forms.Core.Validation;

namespace Neptuo.Forms.Core.Service
{
    public class ProjectService : IProjectService
    {
        [Dependency]
        public IActivityService ActivityService { get; set; }

        [Dependency]
        public IRepository<Project> Repository { get; set; }

        [Dependency]
        public UserContext UserContext { get; set; }

        public IQueryable<Project> GetList()
        {
            return Repository.Where(p => p.OwnerUserID == UserContext.AccountID || p.Managers.Select(u => u.ID).Contains(UserContext.AccountID) || p.Readers.Select(u => u.ID).Contains(UserContext.AccountID)).OrderBy(p => p.ID);
        }

        public Project Get(int id)
        {
            return Repository.FirstOrDefault(p => p.ID == id && (p.Readers.Select(m => m.ID).Contains(UserContext.AccountID) || p.Managers.Select(m => m.ID).Contains(UserContext.AccountID) || p.OwnerUserID == UserContext.AccountID));
        }

        public CreateProjectStatus CreateProject(string name, string description)
        {
            if (!Validator.CheckName(name))
                return CreateProjectStatus.InvalidName;

            if (GetList().Count() == Validator.MaxUserProjects)
                return CreateProjectStatus.ProjectCountExceeded;

            Project project = new Project
            {
                Name = name,
                Description = description,
                OwnerUserID = UserContext.AccountID,
                Created = DateTime.Now
            };
            Repository.Insert(project);
            ActivityService.ProjectCreated(project.ID);
            return CreateProjectStatus.Created;
        }

        public UpdateProjectStatus UpdateProject(int id, string name, string description)
        {
            if (!Validator.CheckName(name))
                return UpdateProjectStatus.InvalidName;

            Project project = Get(id);
            if (project == null)
                return UpdateProjectStatus.NoSuchProject;

            project.Name = name;
            project.Description = description;
            Repository.Update(project);
            ActivityService.ProjectUpdated(project.ID);
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
            return project != null && (project.Managers.Select(m => m.ID).Contains(UserContext.AccountID) || project.OwnerUserID == UserContext.AccountID);
        }

        public bool IsUserOwner(int id)
        {
            Project project = Get(id);
            return project != null && project.OwnerUserID == UserContext.AccountID;
        }
    }
}
