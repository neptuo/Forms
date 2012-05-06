using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Neptuo.Forms.AzureSupport
{
    public class ActivityLogEntry : TableServiceEntity
    {
        public string Message { get; set; }

        public ActivityLogEntry(string message)
            : base(DateTime.Now.Date.Ticks.ToString(), DateTime.Now.Ticks.ToString())
        {
            Message = message;
        }
    }
}
