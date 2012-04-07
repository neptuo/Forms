using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web
{
    public interface IRemoteAuthProvider
    {
        bool Authenticate(string username, bool createPersistentCookie);
    }
}