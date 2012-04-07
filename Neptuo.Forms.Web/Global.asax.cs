using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Neptuo.Web.Localization;
using Neptuo.Web.Mvc;
using Neptuo.Web.Mvc.Auth;

namespace Neptuo.Forms.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "home", action = "index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        private static void RegisterUnity(UnityContainer container)
        {
            container
                .RegisterType<IAuthProvider, LocalAuthProvider>()
                .RegisterType<IRemoteAuthProvider, RemoteAuthProvider>()
                .RegisterType<IDomainSelector, UrlDomainSelector>();
        }

        private static void RegisterLocales(IResourceService service)
        {

        }

        protected void Application_Start()
        {
            Bootstrapper
                .Initialize()
                .BuildUnity(RegisterUnity)
                .BuildLocalization(RegisterLocales)
                .SetupAreas()
                .SetupGlobalFilters()
                .SetupRoutes(RegisterRoutes)
                .SetupViewEngine(true);
        }
    }
}