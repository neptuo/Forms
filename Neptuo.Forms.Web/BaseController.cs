using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Neptuo.Web.Localization.Mvc3;
using Neptuo.Web.Mvc;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    [Localized]
    public class BaseController : Controller
    {
        /// <summary>
        /// Current user context.
        /// </summary>
        [Dependency]
        public UserContext UserContext { get; set; }

        protected override void ExecuteCore()
        {
            LocalizationSelector.Select(ControllerContext.HttpContext.Request, ControllerContext.HttpContext.Response, ControllerContext.RouteData);
            base.ExecuteCore();
        }
    }
}