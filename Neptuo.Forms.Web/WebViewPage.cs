using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    public class WebViewPage<TModel> : Neptuo.Web.Mvc.WebViewPage<TModel>
    {
        /// <summary>
        /// Current user context.
        /// </summary>
        [Dependency]
        public UserContext UserContext { get; set; }


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
            string c = ((string)ViewContext.RouteData.Values["controller"]).ToLowerInvariant();
            string a = ((string)ViewContext.RouteData.Values["action"]).ToLowerInvariant();

            return c == controller && (a == action || action == "*");
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