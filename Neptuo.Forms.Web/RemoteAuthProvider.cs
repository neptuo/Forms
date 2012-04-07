using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.Practices.Unity;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;

namespace Neptuo.Forms.Web
{
    public class RemoteAuthProvider : IRemoteAuthProvider
    {
        [Dependency]
        public IUserService Service { get; set; }

        public bool Authenticate(string username, bool createPersistentCookie)
        {
            UserAccount user = Service.GetByRemoteCredentials(username);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
                return true;
            }
            return false;
        }
    }
}