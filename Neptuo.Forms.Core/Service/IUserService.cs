using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    public interface IUserService
    {
        UserAccount CreateAccount(string username, string password, string email);

        void UpdateAccount(string fullname, string email);

        UserAccount Get(int id);

        IQueryable<UserAccount> GetList();

        UserAccount GetByLocalCredentials(string username, string password);

        UserAccount GetByRemoteCredentials(string username);
    }
}
