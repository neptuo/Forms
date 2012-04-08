using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    public class FieldDefinition : BaseObject
    {
        public string Name { get; set; }

        public int FieldType { get; set; }

        public bool Required { get; set; }

        public int FormDefinitionID { get; set; }
        public virtual FormDefinition FormDefinition { get; set; }
    }
}
