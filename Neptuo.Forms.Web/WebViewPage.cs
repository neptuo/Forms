using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    public class WebViewPage<TModel> : Neptuo.Web.Mvc.WebViewPage<TModel>
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


        /// <summary>
        /// Localizes passed <paramref name="textToTranslate"/>.
        /// </summary>
        /// <param name="textToTranslate">Text to localize.</param>
        /// <returns>Localizes version of <paramref name="textToTranslate"/>.</returns>
        public string Loc(string textToTranslate)
        {
            return (L)textToTranslate;
        }

        /// <summary>
        /// Returns true, if side is shown on current page.
        /// </summary>
        /// <returns><code>true</code>, if side is shown on current page. <code>false</code> otherwise.</returns>
        public bool IsSideBar()
        {
            return ViewBag.HideSideBar == null || !ViewBag.HideSideBar;
        }

        /// <summary>
        /// Returns true, if passed <paramref name="controller"/> and <paramref name="action"/> are current controller and action.
        /// </summary>
        /// <param name="controller">Controller to test.</param>
        /// <param name="action">Action to test.</param>
        /// <returns><code>true</code>, if passed <paramref name="controller"/> and <paramref name="action"/> are current controller and action. <code>false</code> otherwise.</returns>
        public bool IsCurrent(string controller, string action)
        {
            return (string)ViewContext.RouteData.Values["controller"] == controller && ((string)ViewContext.RouteData.Values["action"] == action || action == "*");
        }


        /// <summary>
        /// WTF?
        /// </summary>
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}