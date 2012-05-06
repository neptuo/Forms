using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public class DefaultMessageFormatter : IMessageFormatter
    {
        public string Format(string message, params object[] parameters)
        {
            try
            {
                message = String.Format(message, parameters);
            }
            catch (FormatException) { }

            return message;
        }
    }
}
