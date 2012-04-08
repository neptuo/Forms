using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Neptuo.Web.Validation;

namespace Neptuo.Forms.Web.Models
{
    public class ChangeAccountModel
    {
        [Display(Name = "Fullname")]
        [Required(ErrorMessage = "Fullname is required!")]
        public string Fullname { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required!")]
        [Email(ErrorMessage = "Provide a valid email address!")]
        public string Email { get; set; }
    }
}