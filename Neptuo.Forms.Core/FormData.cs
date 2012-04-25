using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Represents single instance of inserted data.
    /// </summary>
    public class FormData : BaseObject
    {
        /// <summary>
        /// Insertion datetime.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Public Identifier
        /// </summary>
        public string PublicIdentifier { get; set; }

        /// <summary>
        /// Custom tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Form definition.
        /// </summary>
        [ForeignKey("FormDefinition")]
        public int FormDefinitionID { get; set; }
        public FormDefinition FormDefinition { get; set; }

        /// <summary>
        /// Fields.
        /// </summary>
        [InverseProperty("FormData")]
        public virtual List<FieldData> Fields { get; set; }

        /// <summary>
        /// Parent inserted data (for tree structure).
        /// </summary>
        public int? ParentFormDataID { get; set; }
        public virtual FormData ParentFormData { get; set; }
    }
}
