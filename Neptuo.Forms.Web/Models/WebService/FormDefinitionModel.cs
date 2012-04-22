using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web.Models.WebService
{
    public class FormDefinitionModel
    {
        public string PublicIdentifier { get; set; }

        public bool PublicContent { get; set; }

        public string Type { get; set; }

        public IEnumerable<FieldDefinitionModel> Fields { get; set; }
    }

    public class FieldDefinitionModel
    {
        public string PublicIdentifier { get; set; }

        public string Name { get; set; }

        public bool Required { get; set; }

        public string Type { get; set; }

        public string TargetFormPublicIdentifier { get; set; }

        public string TargetFieldPublicIdentifier { get; set; }
    }
}