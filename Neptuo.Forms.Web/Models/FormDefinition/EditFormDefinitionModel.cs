using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Neptuo.Forms.Web.Models
{
    public class EditFormDefinitionModel
    {
        [HiddenInput(DisplayValue = false)]
        public int FormDefinitionID { get; set; }

        [Display(Name = "Form name")]
        [Required(ErrorMessage = "Form name is required!")]
        public string Name { get; set; }

        [Display(Name = "Type")]
        public int FormType { get; set; }

        [Display(Name = "Public content")]
        public bool PublicContent { get; set; }

        [Display(Name = "Project")]
        public int ProjectID { get; set; }

        public IEnumerable<LightProject> Projects { get; set; }

        public IEnumerable<SelectListItem> GetProjects()
        {
            foreach (LightProject project in Projects) {
                yield return new SelectListItem
                {
                    Value = project.ID.ToString(),
                    Text = project.Name,
                    Selected = ProjectID == project.ID
                };
            }
        }

        public IEnumerable<SelectListItem> GetFormTypes()
        {
            foreach (KeyValuePair<int, string> item in Core.FormType.GetTypes())
            {
                yield return new SelectListItem
                {
                    Text = (L)item.Value,
                    Value = item.Key.ToString(),
                    Selected = item.Key == FormType
                };
            }
        }

        public bool IsNew()
        {
            return FormDefinitionID == 0;
        }
    }

    public class LightProject
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}