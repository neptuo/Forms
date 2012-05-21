using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.AzureSupport
{
    public class AzureFileStorage : AzureBlobStorageServiceBase, IFileStorage
    {
        public const string ContainerName = "attachments";

        private CloudBlobContainer container;

        public AzureFileStorage()
        {
            container = BlobClient.GetContainerReference(ContainerName);
            container.CreateIfNotExist();
        }

        public byte[] GetData(string filename)
        {
            CloudBlob blob = container.GetBlobReference(filename);
            return blob.DownloadByteArray();
        }

        public string InsertData(byte[] data)
        {
            string filename = Guid.NewGuid().ToString();
            CloudBlob blob = container.GetBlobReference(filename);
            blob.UploadByteArray(data);
            return filename;
        }
    }
}
