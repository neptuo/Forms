using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.AzureSupport
{
    public class AzureLogger : AzureTableStorageServiceBase, ILogger
    {
        public const string TableName = "ActivityLog";

        [Dependency]
        public IMessageFormatter Formatter { get; set; }

        public AzureLogger()
        {
            EnsureTable(TableName);
        }

        public void Log(string message, params object[] parameters)
        {
            TableServiceContext context = CreateContext();
            context.AddObject(TableName, new ActivityLogEntry(Formatter.Format(message, parameters)));
            context.SaveChangesWithRetries();
        }
    }
}
