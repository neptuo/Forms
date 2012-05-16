using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess.EntityFramework;
using Neptuo.Web.Localization;
using Neptuo.Web.Localization.Mvc3;
using Neptuo.Web.Mvc;
using Neptuo.Web.Mvc.Unity;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Core.DataAccess;

namespace Neptuo.Forms.Web
{
    public static class BootstrapperExtensions
    {
        public static Bootstrapper BuildLocalization(this Bootstrapper bootstrapper, Action<IResourceService> initializeService)
        {
            LocalizationService.SetProviders();
            if (initializeService != null)
                initializeService(LocalizationService.Current);

            return bootstrapper;
        }

        public static Bootstrapper BuildFormsCore(this Bootstrapper bootstrapper, Action<UnityContainer> initializeContainer)
        {
            bootstrapper.BuildUnity();
            FormsCore.CreateInstance();
            FormsCore.Instance.RegisterTypes(bootstrapper.UnityContainer);
            initializeContainer(bootstrapper.UnityContainer);
            return bootstrapper;
        }

        public static Bootstrapper RegisterStandard(this Bootstrapper bootstrapper, HttpServerUtility server)
        {
            FileSystemStorage.StoragePath = server.MapPath("~/Storage");
            FormsCore.Instance.BaseUrl = "http://localhost:36258";

            bootstrapper.UnityContainer
                .RegisterType<IFileStorage, FileSystemStorage>()
                .RegisterType<ILogger, TraceLogger>();

            return bootstrapper;
        }

        public static Bootstrapper InitializeDataContext(this Bootstrapper bootstrapper)
        {
            bootstrapper.UnityContainer.RegisterType<DataContext>(new PerHttpRequestLifetimeManager());
            GenericRepositoryHelper<DataContext>.DbContextInitializer = () => bootstrapper.UnityContainer.Resolve<DataContext>();
            return bootstrapper;
        }
    }
}