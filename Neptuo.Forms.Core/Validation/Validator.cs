using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Validation
{
    /// <summary>
    /// Base validation operations.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Minimum password length.
        /// </summary>
        public const int MinPasswordLength = 6;

        /// <summary>
        /// Maximum of projects that user can own.
        /// </summary>
        public const int MaxUserProjects = 25;

        /// <summary>
        /// Checks that <paramref name="value"/> is not null or empty or whitespace.
        /// </summary>
        /// <param name="value">String to test.</param>
        /// <returns><code>true</code> for not null, not empty and not whitespace <paramref name="value"/>, otherwise <code>false</code>.</returns>
        private static bool CheckNotNullEmptyWhitespace(string value)
        {
            return !String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Checks password constrains.
        /// </summary>
        /// <param name="password">Password to test.</param>
        /// <returns><code>true</code> if <paramref name="password"/> is valid, <code>false</code> otherwise.</returns>
        public static bool CheckPassword(string password)
        {
            if (!CheckNotNullEmptyWhitespace(password))
                return false;

            if (password.Length < MinPasswordLength)
                return false;

            return true;
        }

        /// <summary>
        /// Checks constrains for name.
        /// </summary>
        /// <param name="name">Name to test.</param>
        /// <returns><code>true</code> if <paramref name="name"/> is valid, <code>false</code> otherwise.</returns>
        public static bool CheckName(string name)
        {
            return CheckNotNullEmptyWhitespace(name);
        }

        /// <summary>
        /// Checks whether <paramref name="formType"/> is valid form type.
        /// </summary>
        /// <param name="formType">FormType to check.</param>
        /// <returns><code>true</code> if <paramref name="formType"/> is valid form type, <code>false</code> otherwise.</returns>
        public static bool CheckFormType(int formType)
        {
            return FormType.Form == formType || FormType.Inquiry == formType;
        }

        /// <summary>
        /// Checks <paramref name="fieldType"/> validity against <paramref name="formType"/>.
        /// Inquiry forms can contain only bool fields!
        /// </summary>
        /// <param name="fieldType">FieldType to check.</param>
        /// <param name="formType">FormType of field form definition.</param>
        /// <returns><code>true</code> if <paramref name="fieldType"/> is valid field type, <code>false</code> otherwise.</returns>
        public static bool CheckFieldType(int fieldType, int formType)
        {
            if (!CheckFormType(formType))
                return false;

            if (formType == FormType.Form)
            {
                if (fieldType != FieldType.BoolField && fieldType != FieldType.DoubleField
                    && fieldType != FieldType.FileField && fieldType != FieldType.ReferenceField && fieldType != FieldType.StringField)
                {
                    return false;
                }
            }
            else if(formType == FormType.Inquiry)
            {
                return fieldType == FieldType.BoolField;
            }

            return true;
        }
    }
}
