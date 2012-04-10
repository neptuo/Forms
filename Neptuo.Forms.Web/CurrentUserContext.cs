using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neptuo.Web.Mvc.Auth;
using Neptuo.Web.DataAccess;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    /// <summary>
    /// User context of current user.
    /// </summary>
    public class CurrentUserContext : Neptuo.Forms.Core.UserContext, IUserRoleResolver
    {
        public CurrentUserContext()
            : base(CurrentAccount)
        { }

        /// <summary>
        /// Use with special care!!
        /// Creates new instance on every call!
        /// </summary>
        internal static UserAccount CurrentAccount
        {
            get
            {
                //TODO: Highly critical, resolve there circular dependency!!
                IRepository<UserAccount> service = DependencyResolver.Current.GetService<IRepository<UserAccount>>();
                return service.FirstOrDefault(u => (u.LocalCredentials != null && u.LocalCredentials.Username == HttpContext.Current.User.Identity.Name) || (u.RemoteCredentials != null && u.RemoteCredentials.Username == HttpContext.Current.User.Identity.Name));
            }
        }

        public bool IsUserInRole(string role)
        {
            if (!IsAuthenticated())
                return false;

            if (role == UserRole.Admin)
                return IsAdmin();

            return true;
        }
    }
}