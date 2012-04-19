using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Double field.
    /// </summary>
    public class DoubleFieldData : FieldData<double>
    { }

    /// <summary>
    /// String field.
    /// </summary>
    public class StringFieldData : FieldData<string>
    { }

    /// <summary>
    /// Bool fields.
    /// </summary>
    public class BoolFieldData : FieldData<bool>
    { }

    /// <summary>
    /// File field.
    /// </summary>
    public class FileFieldData : FieldData
    {
        public string Filename { get; set; }

        public string MimeType { get; set; }

        public string LocalFilename { get; set; }

        public override string GetDisplayValue()
        {
            return Filename;
        }
    }

    /// <summary>
    /// Reference field.
    /// </summary>
    public class ReferenceFieldData : FieldData<FormData>
    {
        [ForeignKey("Data")]
        public int DataID { get; set; }

        public override string GetDisplayValue()
        {
            //TODO: Check.
            //return Data.Fields.First(f => f.FieldDefinitionID == FieldDefinition.ReferenceDisplayFieldID).GetDisplayValue();
            return null;
        }
    }
}
