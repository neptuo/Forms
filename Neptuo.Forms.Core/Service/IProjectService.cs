using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public interface IProjectService
    {
        IQueryable<Project> GetList();

        Project Get(int id);

        CreateProjectStatus CreateProject(string name, string description);

        UpdateProjectStatus UpdateProject(int id, string name, string description);

        bool CanUserRead(int id);

        bool CanUserManage(int id);

        bool IsUserOwner(int id);
    }

    public enum CreateProjectStatus
    {
        Created, InvalidName, ProjectCountExceeded
    }

    public enum UpdateProjectStatus
    {
        Updated, InvalidName, NoSuchProject
    }
}
