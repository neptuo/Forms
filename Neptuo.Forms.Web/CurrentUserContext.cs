using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    public class CurrentUserContext : Neptuo.Forms.Core.UserContext
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
                IUserService service = DependencyResolver.Current.GetService<IUserService>();
                return service.Get(HttpContext.Current.User.Identity.Name);
            }
        }
    }
}