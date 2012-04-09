using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web.Models
{
    public class ListFormDefinitionModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string PublicIdentifier { get; set; }

        public DateTime Created { get; set; }

        public bool PublicContent { get; set; }

        public int FormType { get; set; }
    }
}