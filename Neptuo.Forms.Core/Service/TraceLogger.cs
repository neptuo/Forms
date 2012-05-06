using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.Unity;

namespace Neptuo.Forms.Core.Service
{
    public class TraceLogger : ILogger
    {
        [Dependency]
        public IMessageFormatter Formatter { get; set; }

        public void Log(string message, params object[] parameters)
        {
            Trace.WriteLine(String.Format("{0}\t{1}", DateTime.Now, Formatter.Format(message, parameters)));
        }
    }
}
