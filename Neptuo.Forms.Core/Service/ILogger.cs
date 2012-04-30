using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs <paramref name="message"/> with <paramref name="parameters"/> to log.
        /// </summary>
        /// <param name="message">Message, or message format.</param>
        /// <param name="parameters">Optional parameters.</param>
        void Log(string message, params object[] parameters);
    }
}
