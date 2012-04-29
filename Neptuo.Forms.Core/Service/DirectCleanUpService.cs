using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Directly (synchronously) deletes data.
    /// TODO: Use single transaction in deleting (implement in GenericRepository)!
    /// </summary>
    public class DirectCleanUpService : ICleanUpService
    {
        [Dependency]
        public IProjectService ProjectService { get; set; }

        [Dependency]
        public UserContext UserContext { get; set; }

        [Dependency]
        public IRepository<Project> ProjectRepository { get; set; }

        [Dependency]
        public IRepository<FormDefinition> FormDefinitionRepository { get; set; }

        [Dependency]
        public IRepository<FieldDefinition> FieldDefinitionRepository { get; set; }

        [Dependency]
        public IRepository<FormData> FormDataRepository { get; set; }

        [Dependency]
        public IRepository<FieldData> FieldDataRepository { get; set; }


        public DeleteProjectStatus DeleteProject(int projectID)
        {
            Project project = ProjectRepository.Get(projectID);
            if (project != null)
            {
                if (ProjectService.IsUserOwner(projectID))
                {
                    IQueryable<int> forms = FormDefinitionRepository.Where(f => f.ProjectID == projectID).Select(f => f.ID);
                    foreach (int formID in forms)
                        DeleteFormDefinition(formID);

                    ProjectRepository.Delete(project);
                    return DeleteProjectStatus.Deleted;
                }
                return DeleteProjectStatus.PermissionDenied;
            }
            return DeleteProjectStatus.NoSuchProject;
        }

        public DeleteFormDefinitionStatus DeleteFormDefinition(int formDefinitionID)
        {
            FormDefinition form = FormDefinitionRepository.Get(formDefinitionID);
            if (form != null)
            {
                if (ProjectService.CanUserManage(form.ProjectID))
                {
                    IQueryable<int> data = FormDataRepository.Where(d => d.FormDefinitionID == formDefinitionID).Select(d => d.ID);
                    foreach (int itemID in data)
                        DeleteFormData(itemID);

                    foreach (FieldDefinition field in form.Fields)
                        FieldDefinitionRepository.Delete(field);

                    FormDefinitionRepository.Delete(form);
                    return DeleteFormDefinitionStatus.Deleted;
                }
                return DeleteFormDefinitionStatus.PermissionDenied;
            }
            return DeleteFormDefinitionStatus.NoSuchFormDefinition;
        }

        public DeleteFormDataStatus DeleteFormData(int formDataID)
        {
            FormData data = FormDataRepository.Get(formDataID);
            if (data != null)
            {
                if (ProjectService.CanUserManage(data.FormDefinition.ProjectID))
                {
                    foreach (FieldData field in data.Fields)
                        FieldDataRepository.Delete(field);

                    FormDataRepository.Delete(data);
                    return DeleteFormDataStatus.Deleted;
                }
                return DeleteFormDataStatus.PermissionDenied;
            }
            return DeleteFormDataStatus.NoSuchFormData;
        }
    }
}
