using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Base field data.
    /// </summary>
    public abstract class FieldData : BaseObject
    {
        [ForeignKey("FormData")]
        public int FormDataID { get; set; }
        public FormData FormData { get; set; }

        /// <summary>
        /// Field definition.
        /// </summary>
        [ForeignKey("FieldDefinition")]
        public int FieldDefinitionID { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        /// <summary>
        /// Returns data representation for displaying.
        /// </summary>
        /// <returns>Data representation for displaying.</returns>
        public abstract string GetDisplayValue();
    }

    /// <summary>
    /// Base generic field data.
    /// </summary>
    //public abstract class FieldData<T> : FieldData
    //{
    //    /// <summary>
    //    /// Field data.
    //    /// </summary>
    //    public abstract T Data { get; set; }

    //    public override string GetDisplayValue()
    //    {
    //        return Data.ToString();
    //    }
    //}
}
