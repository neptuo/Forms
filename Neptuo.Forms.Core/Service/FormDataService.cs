using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;

namespace Neptuo.Forms.Core.Service
{
    public class FormDataService : IFormDataService
    {
        [Dependency]
        public IRepository<FormData> DataRepository { get; set; }

        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        public IQueryable<FormData> GetList(int formDefinitionID)
        {
            throw new NotImplementedException();
        }

        public FormData Get(int id)
        {
            throw new NotImplementedException();
        }

        public IFormDataCreator Create()
        {
            return new FormDataCreator(DataRepository, FormService);
        }
    }

    public class FormDataCreator : IFormDataCreator
    {
        private IRepository<FormData> dataRepository { get; set; }
        private IFormDefinitionService formService { get; set; }

        private FormDefinition formDefinition;
        private FormData formData;
        private List<FieldData> fieldData;

        public FormDataCreator(IRepository<FormData> dataRepository, IFormDefinitionService formService)
        {
            this.dataRepository = dataRepository;
            this.formService = formService;

            fieldData = new List<FieldData>();
        }

        public SetPublicIdentifierStatus PublicIdentifier(string identifier)
        {
            formDefinition = formService.Get(identifier);
            if (formDefinition == null)
                return SetPublicIdentifierStatus.NoSuchFormDefinition;

            formData = new FormData
            {
                FormDefinitionID = formDefinition.ID
            };

            return SetPublicIdentifierStatus.Set;
        }

        public void Tag(string tag)
        {
            if (formData != null)
                formData.Tag = tag;
        }

        public AddFieldStatus AddFieldConvert(string identifier, string value)
        {
            if (formDefinition == null)
                return AddFieldStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddFieldStatus.NoSuchFieldDefinition;

            if (field.FieldType == FieldType.DoubleField)
            {
                double d;
                if (Double.TryParse(value, out d))
                    return AddField(identifier, d);
                else
                    return AddFieldStatus.IncorrectValue;
            }
            else if (field.FieldType == FieldType.BoolField)
            {
                bool b;
                if (Boolean.TryParse(value, out b))
                    return AddField(identifier, b);
                else
                    return AddFieldStatus.IncorrectValue;
            }
            else if (field.FieldType == FieldType.StringField)
            {
                return AddField(identifier, value);
            }

            return AddFieldStatus.IncorrectFieldType;
        }

        public AddFieldStatus AddField(string identifier, double value)
        {
            if (formDefinition == null)
                return AddFieldStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddFieldStatus.NoSuchFieldDefinition;

            if (field.FieldType != FieldType.DoubleField)
                return AddFieldStatus.IncorrectFieldType;

            formData.Fields.Add(new DoubleFieldData
            {
                Data = value,
                FieldDefinitionID = field.ID,
            });
            return AddFieldStatus.Added;
        }

        public AddFieldStatus AddField(string identifier, string value)
        {
            if (formDefinition == null)
                return AddFieldStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddFieldStatus.NoSuchFieldDefinition;

            if (field.FieldType != FieldType.StringField)
                return AddFieldStatus.IncorrectFieldType;

            formData.Fields.Add(new StringFieldData
            {
                Data = value,
                FieldDefinitionID = field.ID,
            });
            return AddFieldStatus.Added;
        }

        public AddFieldStatus AddField(string identifier, bool value)
        {
            if (formDefinition == null)
                return AddFieldStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddFieldStatus.NoSuchFieldDefinition;

            if (field.FieldType != FieldType.BoolField)
                return AddFieldStatus.IncorrectFieldType;

            formData.Fields.Add(new BoolFieldData
            {
                Data = value,
                FieldDefinitionID = field.ID,
            });
            return AddFieldStatus.Added;
        }

        public AddFieldStatus AddField(string identifier, string filename, string mimetype, byte[] data)
        {
            if (formDefinition == null)
                return AddFieldStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddFieldStatus.NoSuchFieldDefinition;

            if (field.FieldType != FieldType.FileField)
                return AddFieldStatus.IncorrectFieldType;

            throw new NotImplementedException();
            //TODO: Continue..
        }

        public AddReferenceFieldStatus AddReferenceField(string identifier, int selectedID)
        {
            if (formDefinition == null)
                return AddReferenceFieldStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddReferenceFieldStatus.NoSuchFieldDefinition;

            if (field.FieldType != FieldType.ReferenceField)
                return AddReferenceFieldStatus.IncorrectFieldType;

            throw new NotImplementedException();
            //TODO: Continue..
        }

        public CreateFormDataStatus Save()
        {
            if (formDefinition == null || formData == null)
                return CreateFormDataStatus.InvalidCreator;

            dataRepository.Insert(formData);
            return CreateFormDataStatus.Created;
        }
    }

}
