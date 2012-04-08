using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public interface IFormDefinitionService
    {
        IQueryable<FormDefinition> GetList(int projectID);

        FormDefinition Get(int id);

        IQueryable<FieldDefinition> GetFields(int id);

        CreateFormDefinitionStatus CreateForm(string name, int formType, bool publicContent, int projectID);

        UpdateFormDefinitionStatus UpdateForm(int id, string name, bool publicContent);

        CreateFieldDefinitionStatus AddField(int id, string name, int fieldType, bool required);

        UpdateFieldDefinitionStatus UpdateField(int fieldId, string name, bool required);
    }

    public enum CreateFormDefinitionStatus
    {
        Created, InvalidName, InvalidFormType, NoSuchProject
    }

    public enum UpdateFormDefinitionStatus
    {
        Updated, InvalidName, NoSuchFormDefinition
    }

    public enum CreateFieldDefinitionStatus
    {
        Created, InvalidName, InvalidFieldType, NoSuchFormDefinition
    }

    public enum UpdateFieldDefinitionStatus
    {
        Updated, InvalidName, NoSuchFieldDefinition
    }
}
