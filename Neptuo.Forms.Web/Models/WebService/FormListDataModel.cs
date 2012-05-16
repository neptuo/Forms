using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web.Models.WebService
{
    public class FormListDataModel
    {
        public DateTime Created { get; set; }

        public string PublicIdentifier { get; set; }

        public IEnumerable<FieldListDataModel> Fields { get; set; }

        public FormListDataModel()
        {
            Fields = new List<FieldListDataModel>();
        }
    }

    public class FieldListDataModel
    {
        public string PublicIdentifier { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string FileUrl { get; set; }
    }
}