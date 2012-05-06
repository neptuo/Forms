using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Message formatter for activity logger.
    /// </summary>
    public interface IMessageFormatter
    {
        /// <summary>
        /// Tries to format <paramref name="message"/> (template) using <paramref name="parameters"/>.
        /// </summary>
        /// <param name="message">Message or message format.</param>
        /// <param name="parameters">Optional parameters.</param>
        /// <returns>Formatted message.</returns>
        string Format(string message, params object[] parameters);
    }
}
