using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Form types.
    /// </summary>
    public static class FormType
    {
        /// <summary>
        /// Classic form.
        /// </summary>
        public const int Form = 1;

        /// <summary>
        /// Inquiry.
        /// </summary>
        public const int Inquiry = 2;

        /// <summary>
        /// Returns map of ID(storage value of form type)/Name(Constant name).
        /// </summary>
        /// <returns>Map of ID(storage value of form type)/Name(Constant name).</returns>
        public static IEnumerable<KeyValuePair<int, string>> GetTypes()
        {
            yield return new KeyValuePair<int, string>(1, "Form");
            yield return new KeyValuePair<int, string>(2, "Inquiry");
        }
    }
}
