using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neptuo.Forms.AzureSupport
{
    public abstract class AzureStorageServiceBase
    {
        public const string StorageConnectionString = "StorageConnectionString";

        protected static CloudStorageAccount storageAccount;

        static AzureStorageServiceBase()
        {
            storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(StorageConnectionString));
        }
    }
}
