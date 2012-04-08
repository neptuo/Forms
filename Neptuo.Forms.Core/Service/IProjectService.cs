using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public interface IProjectService
    {
        IQueryable<Project> GetList(int userID);

        Project Get(int id, int userID);

        CreateProjectStatus CreateProject(string name, string description, int userID);

        UpdateProjectStatus UpdateProject(int id, string name, string description, int userID);
    }

    public enum CreateProjectStatus
    {
        Created, InvalidName
    }

    public enum UpdateProjectStatus
    {
        Updated, InvalidName, NoSuchProject
    }
}
