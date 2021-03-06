﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;
using Neptuo.Forms.Core.Utils;
using Neptuo.Forms.Core.Validation;

namespace Neptuo.Forms.Core.Service
{
    public class FormDefinitionService : IFormDefinitionService
    {
        [Dependency]
        public IActivityService ActivityService { get; set; }

        [Dependency]
        public IRepository<FormDefinition> FormRepository { get; set; }

        [Dependency]
        public IRepository<FieldDefinition> FieldRepository { get; set; }

        [Dependency]
        public IProjectService ProjectService { get; set; }

        [Dependency]
        public UserContext UserContext { get; set; }

        public IQueryable<FormDefinition> GetList(int projectID)
        {
            if (!ProjectService.CanUserRead(projectID))
                return new FormDefinition[0].AsQueryable();

            return FormRepository.Where(f => f.ProjectID == projectID);
        }

        public FormDefinition Get(int id)
        {
            FormDefinition form = FormRepository.Get(id);
            if (form != null && !ProjectService.CanUserRead(form.ProjectID))
                return null;

            return form;
        }

        public FormDefinition Get(string publicIdentifier)
        {
            FormDefinition form = FormRepository.FirstOrDefault(f => f.PublicIdentifier == publicIdentifier);
            //if (form != null && !ProjectService.CanUserRead(form.ProjectID))
            //    return null;

            return form;
        }

        public IQueryable<FieldDefinition> GetFields(int id)
        {
            FormDefinition form = Get(id);
            if (form != null)
                return form.Fields.AsQueryable();

            return new FieldDefinition[0].AsQueryable();
        }

        public FieldDefinition GetField(int fieldID)
        {
            FieldDefinition field = FieldRepository.Get(fieldID);
            if (field != null && !ProjectService.CanUserRead(field.FormDefinition.ProjectID))
                return null;

            return field;
        }

        public CreateFormDefinitionStatus CreateForm(string name, int formType, bool publicContent, int projectID)
        {
            if (GetList(projectID).Count() == Validator.MaxUserProjects)
                return CreateFormDefinitionStatus.FormCountExceeded;

            if(!Validator.CheckName(name))
                return CreateFormDefinitionStatus.InvalidName;

            if(!Validator.CheckFormType(formType))
                return CreateFormDefinitionStatus.InvalidFormType;

            Project project = ProjectService.Get(projectID);
            if(project == null)
                return CreateFormDefinitionStatus.NoSuchProject;

            if(!ProjectService.CanUserManage(projectID))
                return CreateFormDefinitionStatus.PermissionDenied;

            FormDefinition form = new FormDefinition
            {
                Name = name,
                FormType = formType,
                PublicContent = publicContent,
                ProjectID = projectID,
                PublicIdentifier = HashHelper.ComputePublicIdentifier(typeof(FormDefinition).Name, name),
                Created = DateTime.Now
            };
            FormRepository.Insert(form);
            ActivityService.FormDefinitionCreated(form.ID);
            return CreateFormDefinitionStatus.Created;
        }

        public UpdateFormDefinitionStatus UpdateForm(int id, string name, bool publicContent)
        {
            if(!Validator.CheckName(name))
                return UpdateFormDefinitionStatus.InvalidName;

            FormDefinition form = Get(id);
            if (form == null)
                return UpdateFormDefinitionStatus.NoSuchFormDefinition;

            if (!ProjectService.CanUserManage(form.ProjectID))
                return UpdateFormDefinitionStatus.PermissionDenied;

            form.Name = name;
            form.PublicContent = publicContent;
            FormRepository.Update(form);
            ActivityService.FormDefinitionUpdated(form.ID);
            return UpdateFormDefinitionStatus.Updated;
        }

        public CreateFieldDefinitionStatus AddField(int id, string name, int fieldType, bool required)
        {
            FormDefinition form = Get(id);
            if (form == null)
                return CreateFieldDefinitionStatus.NoSuchFormDefinition;

            if (!Validator.CheckFieldType(fieldType, form.FormType) && fieldType != FieldType.ReferenceField)
                return CreateFieldDefinitionStatus.InvalidFieldType;

            if (String.IsNullOrEmpty(name))
                return CreateFieldDefinitionStatus.InvalidName;

            if (!ProjectService.CanUserManage(form.ProjectID))
                return CreateFieldDefinitionStatus.PermissionDenied;

            form.Fields.Add(new FieldDefinition
            {
                Name = name,
                PublicIdentifier = HashHelper.ComputePublicIdentifier(typeof(FieldDefinition).Name, name),
                FieldType = fieldType,
                Required = required,
                FormDefinitionID = id
            });
            FormRepository.Update(form);
            ActivityService.FieldDefinitionCreated(form.Fields.Max(f => f.ID));
            return CreateFieldDefinitionStatus.Created;
        }

        public CreateFieldDefinitionStatus AddReferenceField(int id, string name, bool required, int targetFieldDefinitionID)
        {
            FormDefinition form = Get(id);
            if (form == null)
                return CreateFieldDefinitionStatus.NoSuchFormDefinition;

            if (String.IsNullOrEmpty(name))
                return CreateFieldDefinitionStatus.InvalidName;

            FieldDefinition targetField = FieldRepository.Get(targetFieldDefinitionID);
            if (targetField == null)
                return CreateFieldDefinitionStatus.NoSuchTargetFieldDefinition;

            FormDefinition targetForm = Get(targetField.FormDefinitionID);
            if (targetForm == null)
                return CreateFieldDefinitionStatus.NoSuchTargetFormDefinition;

            if (!ProjectService.CanUserManage(form.ProjectID))
                return CreateFieldDefinitionStatus.PermissionDenied;

            form.Fields.Add(new FieldDefinition
            {
                Name = name,
                PublicIdentifier = HashHelper.ComputePublicIdentifier(typeof(FieldDefinition).Name, name),
                FieldType = FieldType.ReferenceField,
                Required = required,
                FormDefinitionID = id,
                ReferenceFormID = targetForm.ID,
                ReferenceDisplayFieldID = targetFieldDefinitionID
            });
            FormRepository.Update(form);
            ActivityService.FieldDefinitionCreated(form.Fields.Max(f => f.ID));
            return CreateFieldDefinitionStatus.Created;
        }

        public UpdateFieldDefinitionStatus UpdateField(int fieldId, string name, bool required)
        {
            FieldDefinition field = FieldRepository.Get(fieldId);
            if (Get(field.FormDefinitionID) == null)
                return UpdateFieldDefinitionStatus.NoSuchFieldDefinition;

            if (!Validator.CheckName(name))
                return UpdateFieldDefinitionStatus.InvalidName;

            if (!ProjectService.CanUserManage(field.FormDefinition.ProjectID))
                return UpdateFieldDefinitionStatus.PermissionDenied;

            field.Name = name;
            field.Required = required;
            FieldRepository.Update(field);
            ActivityService.FieldDefinitionUpdated(field.ID);
            return UpdateFieldDefinitionStatus.Updated;
        }
    }
}
