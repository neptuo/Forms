using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.Practices.Unity;
using Neptuo.Web.Mvc.Auth;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    public class LocalAuthProvider : IAuthProvider
    {
        [Dependency]
        public IUserService UserService { get; set; }

        public bool Authenticate(string username, string password, bool createPersistentCookie = false, string domain = null)
        {
            UserAccount user = UserService.GetByLocalCredentials(username, password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
                return true;
            }
            return false;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}