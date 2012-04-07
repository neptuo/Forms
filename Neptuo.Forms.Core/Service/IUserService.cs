using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public interface IUserService
    {
        UserAccount CreateUserAcount(/*Parameters*/);

        UserAccount Get(int id);
    }
}
