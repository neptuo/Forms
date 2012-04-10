using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Neptuo.Forms.Web.Models
{
    public class ListFormDefinitionModel
    {
        public IEnumerable<ListItemFormDefinitionModel> Forms { get; set; }

        public IEnumerable<LightProject> Projects { get; set; }

        public int CurrentProjectID {get;set;}

        public IEnumerable<SelectListItem> GetProjects(UrlHelper url)
        {
            foreach (LightProject project in Projects)
            {
                yield return new SelectListItem
                {
                    Text = project.Name,
                    Value = url.Action("forms", "project", new { projectID = project.ID }),
                    Selected = project.ID == CurrentProjectID
                };
            }
        }
    }

    public class ListItemFormDefinitionModel
    {
        public int FormDefinitionID { get; set; }

        public string Name { get; set; }

        public string PublicIdentifier { get; set; }

        public DateTime Created { get; set; }

        public bool PublicContent { get; set; }

        public int FormType { get; set; }
    }
}