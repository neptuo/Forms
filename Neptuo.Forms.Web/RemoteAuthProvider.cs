using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Neptuo.Forms.Web
{
    public class RemoteAuthProvider : IRemoteAuthProvider
    {
        public bool Authenticate(string username, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
            return true;
        }
    }
}