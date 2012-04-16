using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    public class FormDefinition : BaseObject
    {
        public string Name { get; set; }

        public string PublicIdentifier { get; set; }

        public int FormType { get; set; }

        public DateTime Created { get; set; }

        public bool PublicContent { get; set; }

        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [InverseProperty("FormDefinition")]
        public virtual List<FieldDefinition> Fields { get; set; }
    }
}
