using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;
using Neptuo.Forms.Core.Utils;

namespace Neptuo.Forms.Core.Service
{
    public class FormDataService : IFormDataService
    {
        [Dependency]
        public IRepository<FormData> DataRepository { get; set; }

        [Dependency]
        public IRepository<FieldData> FieldDataRepository { get; set; }

        [Dependency]
        public IFormDefinitionService FormService { get; set; }

        [Dependency]
        public IFileStorage FileStorage { get; set; }

        public IQueryable<FormData> GetList(int formDefinitionID)
        {
            return DataRepository.Where(d => d.FormDefinitionID == formDefinitionID).OrderByDescending(d => d.Created);
        }

        public FormData Get(int id)
        {
            throw new NotImplementedException();
        }

        public FileFieldData GetFileData(int fieldID)
        {
            return FieldDataRepository.FirstOrDefault(f => f.ID == fieldID) as FileFieldData;
        }

        public IFormDataCreator CreateForm()
        {
            return new FormDataCreator(DataRepository, FormService, FileStorage);
        }

        public IInquiryDataCreator CreateInquiry()
        {
            return new InquiryDataCreator(DataRepository, FormService);
        }
    }

    public abstract class CreatorBase
    {
        protected IRepository<FormData> dataRepository;
        protected IFormDefinitionService formService;
        protected int formType;

        protected FormDefinition formDefinition;
        protected FormData formData;

        public CreatorBase(IRepository<FormData> dataRepository, IFormDefinitionService formService, int formType)
        {
            this.dataRepository = dataRepository;
            this.formService = formService;
            this.formType = formType;
        }

        public SetPublicIdentifierStatus PublicIdentifier(string identifier)
        {
            formDefinition = formService.Get(identifier);
            if (formDefinition == null)
                return SetPublicIdentifierStatus.NoSuchFormDefinition;

            if (formDefinition.FormType != formType)
                return SetPublicIdentifierStatus.InvalidFormType;

            formData = new FormData
            {
                FormDefinitionID = formDefinition.ID,
                PublicIdentifier = HashHelper.ComputePublicIdentifier(typeof(FormData).Name, formDefinition.ID.ToString()),
                Created = DateTime.Now,
                Fields = new List<FieldData>()
            };

            return SetPublicIdentifierStatus.Set;
        }

        public void Tag(string tag)
        {
            if (formData != null)
                formData.Tag = tag;
        }

        public CreateFormDataStatus Save()
        {
            if (formDefinition == null || formData == null)
                return CreateFormDataStatus.InvalidCreator;

            dataRepository.Insert(formData);
            return CreateFormDataStatus.Created;
        }
    }

    public class FormDataCreator : CreatorBase, IFormDataCreator
    {
        private IFileStorage fileStorage;

        public FormDataCreator(IRepository<FormData> dataRepository, IFormDefinitionService formService, IFileStorage fileStorage)
            : base(dataRepository, formService, FormType.Form)
        {
            this.fileStorage = fileStorage;
        }

        public SetParentDataStatus Parent(string parentIdentifier)
        {
            if (formDefinition == null)
                return SetParentDataStatus.NoSuchFormDefinition;

            FormData parent = dataRepository.FirstOrDefault(d => d.PublicIdentifier == parentIdentifier);
            if (parent == null)
                return SetParentDataStatus.NoSuchFormData;

            formData.ParentFormDataID = parent.ID;
            return SetParentDataStatus.Set;
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
            else if (field.FieldType == FieldType.ReferenceField)
            {
                return AddReferenceField(identifier, value);
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

            if (field.Required && String.IsNullOrEmpty(value))
                return AddFieldStatus.IncorrectValue;

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

            formData.Fields.Add(new FileFieldData
            {
                Filename = filename,
                MimeType = mimetype,
                LocalFilename = fileStorage.InsertData(data),
                FieldDefinitionID = field.ID,
            });
            return AddFieldStatus.Added;
        }

        public AddFieldStatus AddReferenceField(string identifier, string selectedIdentifier)
        {
            if (formDefinition == null)
                return AddFieldStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddFieldStatus.NoSuchFieldDefinition;

            if (field.FieldType != FieldType.ReferenceField)
                return AddFieldStatus.IncorrectFieldType;

            FormData selected = dataRepository.FirstOrDefault(d => d.PublicIdentifier == selectedIdentifier);
            if (selected == null)
                return AddFieldStatus.NoSuchFormData;

            formData.Fields.Add(new ReferenceFieldData
            {
                ReferenceDataID = selected.ID,
                FieldDefinitionID = field.ID
            });
            return AddFieldStatus.Added;
        }
    }

    public class InquiryDataCreator : CreatorBase, IInquiryDataCreator
    {
        public InquiryDataCreator(IRepository<FormData> dataRepository, IFormDefinitionService formService)
            : base(dataRepository, formService, FormType.Inquiry)
        { }

        public AddInquiryAnswerStatus AddAnswer(string identifier)
        {
            if (formDefinition == null)
                return AddInquiryAnswerStatus.NoSuchFormDefinition;

            FieldDefinition field = formDefinition.Fields.FirstOrDefault(f => f.PublicIdentifier == identifier);
            if (field == null)
                return AddInquiryAnswerStatus.NoSuchFieldDefinition;

            formData.Fields.Add(new BoolFieldData
            {
                Data = true,
                FieldDefinitionID = field.ID
            });
            return AddInquiryAnswerStatus.Added;
        }
    }
}
