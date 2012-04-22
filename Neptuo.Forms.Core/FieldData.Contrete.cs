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
    public class DoubleFieldData : FieldData
    {
        [Column("DoubleData")]
        public double Data { get; set; }

        public override string GetDisplayValue()
        {
            return Data.ToString();
        }
    }

    /// <summary>
    /// String field.
    /// </summary>
    public class StringFieldData : FieldData
    {
        [Column("StringData")]
        public string Data { get; set; }

        public override string GetDisplayValue()
        {
            return Data.ToString();
        }
    }

    /// <summary>
    /// Bool fields.
    /// </summary>
    public class BoolFieldData : FieldData
    {
        [Column("BoolData")]
        public bool Data { get; set; }

        public override string GetDisplayValue()
        {
            return Data.ToString();
        }
    }

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
    public class ReferenceFieldData : FieldData
    {
        [ForeignKey("Data")]
        public int ReferenceDataID { get; set; }
        public virtual FormData Data { get; set; }

        public override string GetDisplayValue()
        {
            //TODO: Check.
            return Data.Fields.First(f => f.FieldDefinitionID == FieldDefinition.ReferenceDisplayFieldID).GetDisplayValue();
            //return null;
        }
    }
}
