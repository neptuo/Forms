using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    public class FieldDefinition : BaseObject
    {
        public string Name { get; set; }

        public int FieldType { get; set; }

        public bool Required { get; set; }

        [ForeignKey("FormDefinition")]
        public int FormDefinitionID { get; set; }
        public virtual FormDefinition FormDefinition { get; set; }

        [ForeignKey("ReferenceForm")]
        public int? ReferenceFormID { get; set; }
        public virtual FormDefinition ReferenceForm { get; set; }

        [ForeignKey("ReferenceDisplayField")]
        public int? ReferenceDisplayFieldID { get; set; }
        public virtual FieldDefinition ReferenceDisplayField { get; set; }
    }
}
