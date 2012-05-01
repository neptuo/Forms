using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.AzureSupport
{
    public class AzureBlobLogger : ILogger
    {
        public void Log(string message, params object[] parameters)
        {
            message = String.Format(message, parameters);
            throw new NotImplementedException();
        }
    }
}
