using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Neptuo.Web.Localization.Mvc3;
using Neptuo.Web.Mvc;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    [Localized]
    public class BaseController : Controller
    {
        private UserContext userContext;

        /// <summary>
        /// Current user context.
        /// </summary>
        public UserContext UserContext
        {
            get
            {
                if (userContext == null)
                    userContext = new UserContext(UserService.Get(User.Identity.Name));

                return userContext;
            }
        }

        [Dependency]
        public IUserService UserService { get; set; }
    }
}