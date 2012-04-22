using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public interface IFormDataService
    {
        IQueryable<FormData> GetList(int formDefinitionID);

        FormData Get(int id);

        IFormDataCreator Create();
    }

    public interface IFormDataCreator
    {
        SetPublicIdentifierStatus PublicIdentifier(string identifier);

        void Tag(string tag);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to match concrete field type.
        /// Works for DoubleField, StringField, BoolField.
        /// </summary>
        /// <param name="identifier">Field public identifier.</param>
        /// <param name="value">Field value.</param>
        /// <returns>Fluent.</returns>
        AddFieldStatus AddFieldConvert(string identifier, string value);

        AddFieldStatus AddField(string identifier, double value);

        AddFieldStatus AddField(string identifier, string value);

        AddFieldStatus AddField(string identifier, bool value);

        AddFieldStatus AddField(string identifier, string filename, string mimetype, byte[] data);

        AddReferenceFieldStatus AddReferenceField(string identifier, int selectedID);

        CreateFormDataStatus Save();
    }

    public enum SetPublicIdentifierStatus
    {
        Set, NoSuchFormDefinition
    }

    public enum AddFieldStatus
    {
        Added, NoSuchFormDefinition, NoSuchFieldDefinition, IncorrectFieldType, IncorrectValue
    }

    public enum AddReferenceFieldStatus
    {
        NoSuchFormDefinition, NoSuchFieldDefinition, NoSuchFormData, IncorrectFieldType
    }

    public enum CreateFormDataStatus
    {
        Created, InvalidCreator
    }
}
