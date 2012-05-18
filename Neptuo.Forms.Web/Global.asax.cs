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
//using Neptuo.Forms.AzureSupport;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

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
            container
                //.RegisterType<ILogger, TraceLogger>(new PerHttpRequestLifetimeManager())
                //.RegisterType<IFileStorage, MemoryFileStorage>()
                //.RegisterType<ILogger, AzureBlobLogger>(new PerHttpRequestLifetimeManager())
                //.RegisterType<IFileStorage, AzureFileStorage>()
                .RegisterType<UserContext, CurrentUserContext>(new PerHttpRequestLifetimeManager())
                .RegisterType<IUserRoleResolver, CurrentUserContext>(new PerHttpRequestLifetimeManager())
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
            menu.GetLinkText = value => (L)value;

            //Main menu
            menu.Register("MainMenu", new MenuItem
            {
                LinkText = "Features",
                Controller = "Home",
                Action = "Features"
            }, new MenuItem
            {
                LinkText = "Learn",
                Controller = "Home",
                Action = "Learn",
                ActiveOn = c => c.RouteData.Values["action"].ToString().StartsWith("Learn")
            }, new MenuItem
            {
                LinkText = "Examples",
                Controller = "Home",
                Action = "Examples",
                ActiveOn = c => c.RouteData.Values["action"].ToString().StartsWith("Example")
            }, new MenuItem
            {
                LinkText = "News",
                Controller = "Home",
                Action = "News"
            }, new MenuItem
            {
                LinkText = "About",
                Controller = "Home",
                Action = "About"
            });

            //Admin menu
            string[] projectActive = { "Project", "FormDefinition", "FieldDefinition" };
            menu.Register("AdminMenu", new MenuItem
            {
                LinkText = "Settings",
                IconUrl = "admin-settings",
                Controller = "account",
                Action = "settings"
            }, new MenuItem
            {
                LinkText = "User account",
                IconUrl = "admin-users",
                Controller = "account",
                Action = "change"
            }, new MenuItem
            {
                LinkText = "Forms",
                IconUrl = "admin-projects",
                Controller = "FormDefinition",
                Action = "Index",
                ActiveOn = c => projectActive.Contains(c.RouteData.GetController())
            });

            //SuperAdmin menu
            menu.Register("SuperAdminMenu", new MenuItem
            {
                LinkText = "Articles",
                Controller = "Article",
                Action = "Index",
                ActiveOn = c => c.RouteData.GetControllerLower() == "article"
            }, new MenuItem
            {
                LinkText = "User accounts",
                Controller = "Account",
                Action = "List",
                Displayed = c => AuthorizeSuperAdminAttribute.IsSuperAdmin()
            });

            //Learn sidebar menu
            menu.Register("LearnMenu", new MenuItem
            {
                LinkText = "Get started",
                Controller = "Home",
                Action = "Learn"
            }, new MenuItem
            {
                LinkText = "REST API",
                Controller = "Home",
                Action = "LearnRestApi"
            }, new MenuItem
            {
                LinkText = "WebService API",
                Controller = "Home",
                Action = "LearnWebService"
            }, new MenuItem
            {
                LinkText = "Javascript library",
                Controller = "Home",
                Action = "LearnJavascript"
            });

            //Learn sidebar menu
            menu.Register("ExamplesMenu", /*new MenuItem
            {
                LinkText = "Basic",
                Controller = "Home",
                Action = "ExamplesBasic"
            }, */new MenuItem
            {
                LinkText = "Contact form",
                Controller = "Home",
                Action = "ExamplesContactForm"
            }, new MenuItem
            {
                LinkText = "File upload",
                Controller = "Home",
                Action = "ExamplesFileUpload"
            });
        }

        protected void Application_Start()
        {
            Bootstrapper
                .Initialize()
                .BuildFormsCore(RegisterUnity)
                .InitializeDataContext()
                .RegisterStandard(Server)
                //.RegisterAzure()
                .BuildLocalization(RegisterLocales)
                .BuildStandartMenu(RegisterMenu)
                .SetupGlobalFilters()
                .SetupRoutes(RegisterRoutes)
                .SetupViewEngine(true);
        }
    }
}