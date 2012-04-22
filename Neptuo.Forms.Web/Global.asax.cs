using System;
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
using Neptuo.Forms.Web.Models.WebService;

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
                LinkText = "Home",
                Controller = "Home",
                Action = "Index"
            }, new MenuItem
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
                UrlContent = "#",
                Controller = "Settings",
                Action = "Index"
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

            ModelBinders.Binders.Add(typeof(FormInsertModel), new FormInsertModelBinder());
        }
    }
}