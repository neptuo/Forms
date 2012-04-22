using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web.Models.WebService
{
    public class InsertValidationModel
    {
        public string PublicIdentifier { get; set; }

        public string Error { get; set; }

        public InsertValidationModel()
        { }

        public InsertValidationModel(string identifier, string error)
        {
            PublicIdentifier = identifier;
            Error = error;
        }
    }
}