using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.AzureSupport
{
    public class AzureFileStorage : AzureStorageServiceBase, IFileStorage
    {
        public const string TableName = "FileStorage";

        public AzureFileStorage()
        {
            EnsureTable(TableName);
        }

        public byte[] GetData(string filename)
        {
            FileEntry file = CreateContext().CreateQuery<FileEntry>(TableName).FirstOrDefault(e => e.PartitionKey == FileEntry.DefaultPartitionKey && e.RowKey == filename);
            if (file == null)
                return null;

            return file.Data;
        }

        public string InsertData(byte[] data)
        {
            string filename = Guid.NewGuid().ToString();

            TableServiceContext context = CreateContext();
            context.AddObject(TableName, new FileEntry(filename, data));
            context.SaveChangesWithRetries();

            return filename;
        }
    }
}
