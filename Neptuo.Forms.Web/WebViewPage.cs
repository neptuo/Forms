using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web
{
    public class WebViewPage<TModel> : Neptuo.Web.Mvc.WebViewPage<TModel>
    {
        /// <summary>
        /// Localizes passed <paramref name="textToTranslate"/>.
        /// </summary>
        /// <param name="textToTranslate">Text to localize.</param>
        /// <returns>Localizes version of <paramref name="textToTranslate"/>.</returns>
        public string Loc(string textToTranslate)
        {
            return (L)textToTranslate;
        }

        public bool IsSideBar()
        {
            return ViewBag.HideSideBar == null || !ViewBag.HideSideBar;
        }

        public bool IsCurrent(string controller, string action)
        {
            return (string)ViewContext.RouteData.Values["controller"] == controller && (string)ViewContext.RouteData.Values["action"] == action;
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