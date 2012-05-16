using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;
using Neptuo.Web.Mvc;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.AzureSupport
{
    /// <summary>
    /// Bootstrapper extensions for azure.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Registers <see cref="AzureFileStorage"/> as <see cref="IFileStorage"/>.
        /// Registers <see cref="AzureLogger"/> as <see cref="ILogger"/>.
        /// </summary>
        /// <param name="bootstrapper">Current bootstrapper.</param>
        /// <returns>Fluently.</returns>
        public static Bootstrapper RegisterAzure(this Bootstrapper bootstrapper)
        {
            if (RoleEnvironment.IsAvailable)
            {
                FormsCore.Instance.BaseUrl = "http://neptuo-forms.cloudapp.net";

                bootstrapper.UnityContainer
                    .RegisterType<IFileStorage, AzureFileStorage>()
                    .RegisterType<ILogger, AzureLogger>();
            }

            return bootstrapper;
        }
    }
}
