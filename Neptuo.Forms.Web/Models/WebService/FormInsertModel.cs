using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web.Models.WebService
{
    public class FormInsertModel
    {
        public string FormPublicIdentifier { get; set; }

        public string FormTag { get; set; }

        public List<FieldInsertModel> Fields { get; set; }
    }

    public class FieldInsertModel
    {
        public string PublicIndetifier { get; set; }

        public string Value { get; set; }
    }
}