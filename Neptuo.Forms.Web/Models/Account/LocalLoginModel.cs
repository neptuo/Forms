using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Auth;

namespace Neptuo.Forms.Web.Models
{
    public class LocalLoginModel : ILoginModel
    {
        [Display(Name="Username")]
        [Required(ErrorMessage="Username is required!")]
        public string Username { get; set; }

        [Display(Name="Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password is required!")]
        public string Password { get; set; }
    }
}