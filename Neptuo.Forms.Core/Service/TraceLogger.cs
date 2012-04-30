using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Neptuo.Forms.Core.Service
{
    public class TraceLogger : ILogger
    {
        public void Log(string message, params object[] parameters)
        {
            message = String.Format(message, parameters);
            Trace.WriteLine(String.Format("{0}\t{1}", DateTime.Now, message));
        }
    }
}
