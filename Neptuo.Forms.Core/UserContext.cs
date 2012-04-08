using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Context of user account.
    /// </summary>
    public class UserContext
    {
        private bool isAuthenticated = false;

        /// <summary>
        /// User Account.
        /// </summary>
        public UserAccount Account { get; protected set; }

        /// <summary>
        /// Account ID.
        /// </summary>
        public int AccountID { get { return Account.ID; } }

        /// <summary>
        /// Creates user context from <paramref name="account"/>.
        /// </summary>
        /// <param name="account">User account.</param>
        public UserContext(UserAccount account)
        {
            if (account == null)
            {
                Account = new UserAccount
                {
                    Fullname = "Anonymous"
                };
                isAuthenticated = false;
            }
            else
            {
                Account = account;
                isAuthenticated = true;
            }
        }

        /// <summary>
        /// Determines whether user is admin.
        /// </summary>
        /// <returns><code>true</code> is user is admin, <code>false</code> otherwise.</returns>
        public bool IsAdmin()
        {
            return Account.UserRole == UserRole.Admin;
        }

        /// <summary>
        /// Determines whether user is authenticated.
        /// </summary>
        /// <returns><code>true</code> is user is authenticated, <code>false</code> otherwise.</returns>
        public bool IsAuthenticated()
        {
            return isAuthenticated;
        }

        /// <summary>
        /// Determines whether user has local credentials.
        /// </summary>
        /// <returns><code>true</code> is user has local credentials, <code>false</code> otherwise.</returns>
        public bool HasLocalCredentials()
        {
            if (IsAuthenticated())
                return Account.LocalCredentials != null;

            return false;
        }

        /// <summary>
        /// Determines whether user has remote credentials (from some 3rd party auth service).
        /// </summary>
        /// <returns><code>true</code> is user has remote credentials, <code>false</code> otherwise.</returns>
        public bool HasRemoteCredentials()
        {
            if (IsAuthenticated())
                return Account.RemoteCredentials != null;

            return false;
        }
    }
}
