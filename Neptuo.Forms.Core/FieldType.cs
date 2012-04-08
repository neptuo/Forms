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
    }
}
