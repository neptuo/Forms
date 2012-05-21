using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Neptuo.Forms.AzureSupport
{
    public abstract class AzureBlobStorageServiceBase : AzureStorageServiceBase
    {
        protected CloudBlobClient BlobClient { get; set; }

        public AzureBlobStorageServiceBase()
        {
            BlobClient = storageAccount.CreateCloudBlobClient();
        }
    }
}
