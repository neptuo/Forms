using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neptuo.Forms.Web.Models
{
    public class EditArticleModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "Content is required!")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }

        [Display(Name = "Culture")]
        [Required(ErrorMessage = "Culture is required!")]
        public string Culture { get; set; }

        public bool IsNew()
        {
            return ID == 0;
        }
    }
}