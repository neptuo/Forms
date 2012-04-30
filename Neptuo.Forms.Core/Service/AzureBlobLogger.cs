using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public class AzureBlobLogger : ILogger
    {
        public void Log(string message, params object[] parameters)
        {
            message = String.Format(message, parameters);
        }
    }
}
