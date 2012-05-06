using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Neptuo.Forms.AzureSupport
{
    public abstract class AzureStorageServiceBase
    {
        public const string StorageConnectionString = "StorageConnectionString";

        private static CloudStorageAccount storageAccount;

        protected CloudTableClient TableClient { get; set; }

        static AzureStorageServiceBase()
        {
            storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(StorageConnectionString));
        }

        public AzureStorageServiceBase()
        {
            TableClient = storageAccount.CreateCloudTableClient();
        }

        protected void EnsureTable(string tableName)
        {
            TableClient.CreateTableIfNotExist(tableName);
        }

        protected TableServiceContext CreateContext()
        {
            return TableClient.GetDataServiceContext();
        }
    }
}
