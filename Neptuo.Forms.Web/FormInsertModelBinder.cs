using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neptuo.Forms.Web.Models.WebService;

namespace Neptuo.Forms.Web
{
    public class FormInsertModelBinder : IModelBinder
    {
        private static List<string> NotFieldIdentifiers = new List<string>
        {
            "FormTag", "FormPublicIdentifier"
        };

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            FormInsertModel model = new FormInsertModel();
            model.FormPublicIdentifier = (string)controllerContext.RouteData.Values["FormPublicIdentifier"];
            model.FormTag = controllerContext.HttpContext.Request.Form["FormTag"];
            model.Fields = new List<FieldInsertModel>();

            string httpMethod = controllerContext.HttpContext.Request.HttpMethod;
            NameValueCollection form = httpMethod == "POST" ? controllerContext.HttpContext.Request.Form : controllerContext.HttpContext.Request.QueryString;
            foreach (string key in form.AllKeys)
            {
                if (key.Length == 13 && !key.StartsWith("_") && !String.IsNullOrEmpty(key) && !String.IsNullOrWhiteSpace(key) && !NotFieldIdentifiers.Contains(key))
                {
                    model.Fields.Add(new FieldInsertModel
                    {
                        PublicIndetifier = key,
                        Value = form[key]
                    });
                }
            }

            return model;
        }
    }
}