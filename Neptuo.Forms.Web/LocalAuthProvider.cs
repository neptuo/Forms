using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Neptuo.Web.Mvc.Auth;

namespace Neptuo.Forms.Web
{
    public class LocalAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password, bool createPersistentCookie = false, string domain = null)
        {
            FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
            return true;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}