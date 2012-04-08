using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Forms.Core;

namespace Neptuo.Forms.Web
{
    public class UserContext
    {
        private bool isAuthenticated = false;

        public UserAccount Account { get; set; }

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

        public bool IsAdmin()
        {
            //TODO: Move string constant!!
            return Account.UserRole == UserRole.Admin;
        }

        public bool IsAuthenticated()
        {
            return isAuthenticated;
        }

        public bool HasLocalCredentials()
        {
            if (IsAuthenticated())
                return Account.LocalCredentials != null;

            return false;
        }

        public bool HasRemoteCredentials()
        {
            if (IsAuthenticated())
                return Account.RemoteCredentials != null;

            return false;
        }
    }
}