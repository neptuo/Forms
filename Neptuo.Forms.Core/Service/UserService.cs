using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.DataAccess;
using Neptuo.Forms.Core.Utils;

namespace Neptuo.Forms.Core.Service
{
    public class UserService : IUserService
    {
        public IRepository<UserAccount> Repository { get; set; }

        public UserAccount CreateAccount(string username, string password, string email)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccount(string fullname, string email)
        {
            throw new NotImplementedException();
        }

        public UserAccount Get(int id)
        {
            return Repository.Get(id);
        }

        public IQueryable<UserAccount> GetList()
        {
            return Repository;
        }

        public UserAccount GetByLocalCredentials(string username, string password)
        {
            return Repository.FirstOrDefault(u => u.LocalCredentials.Username == username && u.LocalCredentials.Password == HashHelper.ComputePassword(username, password));
        }

        public UserAccount GetByRemoteCredentials(string username)
        {
            return Repository.FirstOrDefault(u => u.RemoteCredentials.Username == username);
        }
    }
}
