using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Service for managing form definition state. CRU for forms and fields.
    /// </summary>
    public interface IFormDefinitionService
    {
        /// <summary>
        /// Returns list of forms from <paramref name="projectID"/> that current user can access.
        /// </summary>
        /// <param name="projectID">Project ID.</param>
        /// <returns>List of forms from <paramref name="projectID"/> that current user can access.</returns>
        IQueryable<FormDefinition> GetList(int projectID);

        /// <summary>
        /// Returns form definition by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Form definition ID.</param>
        /// <returns>Form definition by <paramref name="id"/>.</returns>
        FormDefinition Get(int id);

        /// <summary>
        /// Returns field definitions of form definition from <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Form definition ID.</param>
        /// <returns>List of fields.</returns>
        IQueryable<FieldDefinition> GetFields(int id);

        /// <summary>
        /// Returns field definition by <paramref name="fieldId"/>.
        /// </summary>
        /// <param name="fieldID">Field definition ID.</param>
        /// <returns></returns>
        FieldDefinition GetField(int fieldID);

        /// <summary>
        /// Creates new form definition.
        /// </summary>
        /// <param name="name">Form name.</param>
        /// <param name="formType">Form type, <see cref="FormType"/>.</param>
        /// <param name="publicContent">Mark form data as public (accessible through web service).</param>
        /// <param name="projectID">Project ID.</param>
        /// <returns>Creation status, <see cref="CreateFormDefinitionStatus"/>.</returns>
        CreateFormDefinitionStatus CreateForm(string name, int formType, bool publicContent, int projectID);

        /// <summary>
        /// Updates existing form definition.
        /// </summary>
        /// <param name="id">Form definition ID to update.</param>
        /// <param name="name">Form name.</param>
        /// <param name="publicContent">Form data as public (accessible through web service).</param>
        /// <returns>Update status, <see cref="UpdateFormDefinitionStatus"/>.</returns>
        UpdateFormDefinitionStatus UpdateForm(int id, string name, bool publicContent);

        /// <summary>
        /// Adds new field to form.
        /// </summary>
        /// <param name="id">Form definition ID.</param>
        /// <param name="name">Field name.</param>
        /// <param name="fieldType">Field type, <see cref="FieldType"/>.</param>
        /// <param name="required">Is field required?</param>
        /// <returns>Creation status, <see cref="CreateFieldDefinitionStatus"/>.</returns>
        CreateFieldDefinitionStatus AddField(int id, string name, int fieldType, bool required);

        /// <summary>
        /// Adds new reference field to form.
        /// </summary>
        /// <param name="id">Form definition ID.</param>
        /// <param name="name">Field name.</param>
        /// <param name="required">Is field required?</param>
        /// <param name="targetFieldDefinitionID">Target form field, that will be displayed to user.</param>
        /// <returns>Creation status, <see cref="CreateFieldDefinitionStatus"/>.</returns>
        CreateFieldDefinitionStatus AddReferenceField(int id, string name, bool required, int targetFieldDefinitionID);

        /// <summary>
        /// Updates form field.
        /// </summary>
        /// <param name="fieldId">Field definition ID.</param>
        /// <param name="name">Field name.</param>
        /// <param name="required">Is field required?</param>
        /// <returns>Update status, <see cref="UpdateFieldDefinitionStatus"/>.</returns>
        UpdateFieldDefinitionStatus UpdateField(int fieldId, string name, bool required);
    }

    /// <summary>
    /// Enumeration of possible states that can occur when creating form definition.
    /// </summary>
    public enum CreateFormDefinitionStatus
    {
        /// <summary>
        /// Form definition was successfully created.
        /// </summary>
        Created, 
        
        /// <summary>
        /// Error state, there was provided invalid name for form.
        /// </summary>
        InvalidName, 
        
        /// <summary>
        /// Error state, invalid form type was provided, <see cref="FormType"/>.
        /// </summary>
        InvalidFormType, 
        
        /// <summary>
        /// Error state, there was provided ID to non-existing projekt.
        /// </summary>
        NoSuchProject
    }

    /// <summary>
    /// Enumeration of possible states that can occur when updating form definition.
    /// </summary>
    public enum UpdateFormDefinitionStatus
    {
        /// <summary>
        /// Form definition was update.
        /// </summary>
        Updated, 
        
        /// <summary>
        /// Error state, there was provided invalid name for form.
        /// </summary>
        InvalidName, 
        
        /// <summary>
        /// Error state, there was provided ID to non-existing form definition.
        /// </summary>
        NoSuchFormDefinition
    }

    /// <summary>
    /// Enumeration of possible states that can occur when creating form field.
    /// </summary>
    public enum CreateFieldDefinitionStatus
    {
        /// <summary>
        /// Field definition created and added to form definition.
        /// </summary>
        Created, 
        
        /// <summary>
        /// Error state, invalid field name was provided.
        /// </summary>
        InvalidName, 
        
        /// <summary>
        /// Error state, invalid field type was provided, <see cref="FieldType"/>.
        /// </summary>
        InvalidFieldType, 
        
        /// <summary>
        /// Error state, there was provided ID to non-existing form definition.
        /// </summary>
        NoSuchFormDefinition, 
        
        /// <summary>
        /// Error state, there was provided ID to non-existing target form definition when creating reference field.
        /// </summary>
        NoSuchTargetFormDefinition, 
        
        /// <summary>
        /// Error state, there was provided ID to non-existing target field definition when creating reference field.
        /// </summary>
        NoSuchTargetFieldDefinition
    }

    /// <summary>
    /// Enumeration of possible states that can occur where updating form field.
    /// </summary>
    public enum UpdateFieldDefinitionStatus
    {
        /// <summary>
        /// Field definition successfully updated.
        /// </summary>
        Updated, 
        
        /// <summary>
        /// Error state, invalid field name was provided.
        /// </summary>
        InvalidName, 
        
        /// <summary>
        /// Error state, there was provided ID to non-existing field definition.
        /// </summary>
        NoSuchFieldDefinition
    }
}
