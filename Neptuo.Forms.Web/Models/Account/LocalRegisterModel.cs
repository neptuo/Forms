﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Neptuo.Forms.Web.Models
{
    public class LocalRegisterModel : RegisterModel
    {
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        [StringLength(100, MinimumLength = Core.Validation.Validator.MinPasswordLength, ErrorMessage = "Minimum password length is {2}!")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm password is required!")]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        [StringLength(100, MinimumLength = Core.Validation.Validator.MinPasswordLength, ErrorMessage = "Minimum password length is {2}!")]
        public string PasswordConfirm { get; set; }
    }
}