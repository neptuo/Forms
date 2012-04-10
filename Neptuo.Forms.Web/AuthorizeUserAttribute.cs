using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Forms.Core;

namespace Neptuo.Forms.Web
{
    public class AuthorizeUserAttribute : Neptuo.Web.Mvc.Auth.AuthorizeUserAttribute
    {
        public AuthorizeUserAttribute()
            : base(UserRole.User, UserRole.Admin)
        { }
    }

    public class AuthorizeAdminAttribute : Neptuo.Web.Mvc.Auth.AuthorizeUserAttribute
    {
        public AuthorizeAdminAttribute()
            : base(UserRole.Admin)
        { }
    }
}