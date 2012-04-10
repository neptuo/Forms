using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neptuo.Forms.Web.Models
{
    public class EditProjectModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ProjectID { get; set; }

        [Display(Name = "Project name")]
        [Required(ErrorMessage = "Project name is required!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool IsNew()
        {
            return ProjectID == 0;
        }
    }
}