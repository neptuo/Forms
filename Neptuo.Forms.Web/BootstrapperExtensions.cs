using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Localization;
using Neptuo.Web.Localization.Mvc3;
using Neptuo.Web.Mvc;

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
    }
}