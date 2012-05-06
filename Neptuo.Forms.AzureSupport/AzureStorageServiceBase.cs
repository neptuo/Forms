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

        protected CloudTableClient TableClient { get; set; }

        public AzureStorageServiceBase()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(StorageConnectionString));

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
