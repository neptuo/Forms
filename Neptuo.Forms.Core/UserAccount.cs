using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Represents register user.
    /// </summary>
    public class UserAccount : BaseObject
    {
        /// <summary>
        /// User fullname.
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Public identifier used to identify user (eg.: for sending invitations).
        /// </summary>
        public string PublicIdentifier { get; set; }

        /// <summary>
        /// User e-mail address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Enabled/Disabled account.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Date account created (user registration date).
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Role of user, <see cref="UserRole"/>.
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// Account's local credentials.
        /// </summary>
        public virtual LocalCredentials LocalCredentials { get; set; }

        /// <summary>
        /// Account's remote credentials.
        /// </summary>
        public virtual RemoteCredentials RemoteCredentials { get; set; }
    }
}
