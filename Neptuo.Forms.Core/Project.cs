using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    public class Project : BaseObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        [ForeignKey("Owner")]
        public int OwnerUserID { get; set; }
        public virtual UserAccount Owner { get; set; }
    }
}
