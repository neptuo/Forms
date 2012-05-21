using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Neptuo.Forms.AzureSupport
{
    public abstract class AzureTableStorageServiceBase : AzureStorageServiceBase
    {
        protected CloudTableClient TableClient { get; set; }

        public AzureTableStorageServiceBase()
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
