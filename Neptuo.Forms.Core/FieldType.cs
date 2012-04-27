using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Possible form field types.
    /// </summary>
    public static class FieldType
    {
        /// <summary>
        /// Number field.
        /// </summary>
        public const int DoubleField = 1;

        /// <summary>
        /// String field.
        /// </summary>
        public const int StringField = 2;

        /// <summary>
        /// True/false field.
        /// </summary>
        public const int BoolField = 3;

        /// <summary>
        /// File attachment field.
        /// </summary>
        public const int FileField = 4;

        /// <summary>
        /// Field as reference to other form definition.
        /// </summary>
        public const int ReferenceField = 5;

        /// <summary>
        /// Returns map of ID(storage value of field type)/Name(Constant name).
        /// </summary>
        /// <returns>Map of ID(storage value of field type)/Name(Constant name).</returns>
        public static IEnumerable<KeyValuePair<int, string>> GetTypes()
        {
            yield return new KeyValuePair<int, string>(DoubleField, "DoubleField");
            yield return new KeyValuePair<int, string>(StringField, "StringField");
            yield return new KeyValuePair<int, string>(BoolField, "BoolField");
            yield return new KeyValuePair<int, string>(FileField, "FileField");
            yield return new KeyValuePair<int, string>(ReferenceField, "ReferenceField");
        }
    }
}
