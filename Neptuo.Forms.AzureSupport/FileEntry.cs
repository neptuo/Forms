using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Neptuo.Forms.AzureSupport
{
    public class FileEntry : TableServiceEntity
    {
        public const string DefaultPartitionKey = "file";

        public byte[] Data { get; set; }

        public FileEntry(string name, byte[] data)
            : base(DefaultPartitionKey, name)
        {
            Data = data;
        }
    }
}
