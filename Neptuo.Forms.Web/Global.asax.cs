﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using RiaLibrary.Web;
using Neptuo.Web.Localization;
using Neptuo.Web.Mvc;
using Neptuo.Web.Mvc.Auth;
using Neptuo.Web.Mvc.Html;
using Neptuo.Web.Mvc.Unity;
using Neptuo.Forms.Core;

namespace Neptuo.Forms.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoutes();

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "home", action = "index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        private void RegisterUnity(UnityContainer container)
        {
            FormsCore.RegisterTypes(container);

            container
                .RegisterType<UserContext, CurrentUserContext>(new PerHttpRequestLifetimeManager())
                .RegisterType<IAuthProvider, LocalAuthProvider>()
                .RegisterType<IRemoteAuthProvider, RemoteAuthProvider>()
                .RegisterType<IDomainSelector, UrlDomainSelector>();
        }

        private void RegisterLocales(IResourceService service)
        {
            service.AddBundle(Server.MapPath("~/Locales"), "FormsWeb");
        }

        private void RegisterMenu(StandartMenuRepository menu)
        {

        }

        protected void Application_Start()
        {
            Bootstrapper
                .Initialize()
                .BuildUnity(RegisterUnity)
                .BuildLocalization(RegisterLocales)
                .BuildStandartMenu(RegisterMenu)
                .SetupGlobalFilters()
                .SetupRoutes(RegisterRoutes)
                .SetupViewEngine(true);
        }
    }
}