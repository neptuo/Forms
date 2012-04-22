using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Neptuo.Web.Localization.Mvc3;
using Neptuo.Web.Mvc;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    [Localized]
    public class BaseController : Neptuo.Web.Mvc.Controller
    {
        /// <summary>
        /// Current user context.
        /// </summary>
        [Dependency]
        public UserContext UserContext { get; set; }

        protected override void ExecuteCore()
        {
            LocalizationSelector.Select(ControllerContext.HttpContext.Request, ControllerContext.HttpContext.Response, ControllerContext.RouteData);

            try
            {
                base.ExecuteCore();
            }
            catch (Exception e)
            {
                IActivityService activity = DependencyResolver.Current.GetService<IActivityService>();
                activity.ErrorThrown(e);
                throw e;
            }
        }
    }
}