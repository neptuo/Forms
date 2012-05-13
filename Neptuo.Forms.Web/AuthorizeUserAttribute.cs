using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Forms.Core;
using System.Web.Mvc;
using Neptuo.Forms.Web.Models;

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

    public class AuthorizeSuperAdminAttribute : AuthorizeAdminAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
                return false;

            return IsSuperAdmin();
        }

        public static bool IsSuperAdmin()
        {
            UserContext userContext = DependencyResolver.Current.GetService<UserContext>();
            return IsSuperAdmin(userContext.Account);
        }

        public static bool IsSuperAdmin(UserAccount account)
        {
            return account.LocalCredentials != null && account.LocalCredentials.Username == FormsCore.AdminUsername;
        }

        public static bool IsSuperAdmin(ListUserAccount account)
        {
            return account.GetUsername() == FormsCore.AdminUsername;
        }
    }
}