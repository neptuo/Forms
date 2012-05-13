using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;
using Neptuo.Forms.Core.Utils;
using Neptuo.Forms.Core.Validation;

namespace Neptuo.Forms.Core.Service
{
    public class UserService : IUserService
    {
        [Dependency]
        public IActivityService ActivityService { get; set; }

        [Dependency]
        public IRepository<UserAccount> Repository { get; set; }

        [Dependency]
        public UserContext UserContext { get; set; }

        public UserCreateStatus CreateAccount(string username, string password, string fullname, string email)
        {
            bool exists = Repository.FirstOrDefault(u => u.LocalCredentials != null && u.LocalCredentials.Username == username) != null;
            if (!exists)
            {
                if (!Validator.CheckPassword(password))
                    return UserCreateStatus.InsufficientPassword;

                UserAccount user = new UserAccount
                {
                    PublicIdentifier = HashHelper.ComputePublicIdentifier(typeof(UserAccount).Name, username),
                    LocalCredentials = new LocalCredentials
                    {
                        Username = username,
                        Password = HashHelper.ComputePassword(username, password)
                    },
                    Enabled = true,
                    Created = DateTime.Now,
                    Fullname = fullname,
                    Email = email,
                    UserRole = UserRole.User
                };
                Repository.Insert(user);
                return UserCreateStatus.Created;
            }
            ActivityService.UserRegistered(username);
            return UserCreateStatus.UsernameUsed;
        }

        public UserCreateStatus CreateAccount(string remoteUsername, string fullname, string email)
        {
            bool exists = Repository.FirstOrDefault(u => u.RemoteCredentials != null && u.RemoteCredentials.Username == remoteUsername) != null;
            if (!exists)
            {
                UserAccount user = new UserAccount
                {
                    PublicIdentifier = HashHelper.ComputePublicIdentifier(typeof(UserAccount).Name, remoteUsername),
                    RemoteCredentials = new RemoteCredentials
                    {
                        Username = remoteUsername
                    },
                    Enabled = true,
                    Created = DateTime.Now,
                    Fullname = fullname,
                    Email = email,
                    UserRole = UserRole.User
                };
                Repository.Insert(user);
                return UserCreateStatus.Created;
            }
            ActivityService.UserRegistered(remoteUsername);
            return UserCreateStatus.UsernameUsed;
        }

        public UserUpdateStatus UpdateAccount(string fullname, string email)
        {
            UserAccount user = Repository.Get(UserContext.AccountID);
            if (user != null)
            {
                user.Fullname = fullname;
                user.Email = email;
                Repository.Update(user);
                return UserUpdateStatus.Updated;
            }
            return UserUpdateStatus.NoSuchUser;
        }

        public ChangePasswordStatus ChangePassword(string currentPassword, string newPassword)
        {
            UserAccount user = Repository.Get(UserContext.AccountID);
            if (user != null)
            {
                if (!Validator.CheckPassword(newPassword))
                    return ChangePasswordStatus.InsuficientComplexity;

                if (user.LocalCredentials == null)
                    return ChangePasswordStatus.NoLocalCredentials;

                currentPassword = HashHelper.ComputePassword(user.LocalCredentials.Username, currentPassword);
                if (user.LocalCredentials.Password != currentPassword)
                    return ChangePasswordStatus.InvalidCurrentPassword;

                user.LocalCredentials.Password = HashHelper.ComputePassword(user.LocalCredentials.Username, newPassword);
                Repository.Update(user);
                return ChangePasswordStatus.Changed;
            }
            return ChangePasswordStatus.NoSuchUser;
        }

        public void DisableUser(int id)
        {
            if (!UserContext.IsAdmin())
                throw new Validation.PermissionDeniedException("User is not admin!");

            UserAccount user = Repository.Get(id);
            if (user != null && user.Enabled)
            {
                user.Enabled = false;
                Repository.Update(user);
            }
        }

        public void EnableUser(int id)
        {
            if (!UserContext.IsAdmin())
                throw new Validation.PermissionDeniedException("User is not admin!");

            UserAccount user = Repository.Get(id);
            if (user != null && !user.Enabled)
            {
                user.Enabled = true;
                Repository.Update(user);
            }
        }

        public void MakeAdmin(int id)
        {
            if (!UserContext.IsAdmin())
                throw new Validation.PermissionDeniedException("User is not admin!");

            UserAccount user = Repository.Get(id);
            if (user != null && user.UserRole != UserRole.Admin)
            {
                user.UserRole = UserRole.Admin;
                Repository.Update(user);
            }
        }

        public void MakeUser(int id)
        {
            if (!UserContext.IsAdmin())
                throw new Validation.PermissionDeniedException("User is not admin!");

            UserAccount user = Repository.Get(id);
            if (user != null && user.UserRole != UserRole.User)
            {
                user.UserRole = UserRole.User;
                Repository.Update(user);
            }
        }

        public UserAccount Get(int id)
        {
            return Repository.Get(id);
        }

        public UserAccount Get(string username)
        {
            return Repository.FirstOrDefault(u => (u.LocalCredentials != null && u.LocalCredentials.Username == username) || (u.RemoteCredentials != null && u.RemoteCredentials.Username == username));
        }

        public UserAccount GetByIdentifier(string publicIdentifier)
        {
            return Repository.FirstOrDefault(u => u.PublicIdentifier == publicIdentifier);
        }

        public IQueryable<UserAccount> GetList()
        {
            return Repository.OrderBy(u => u.ID);
        }

        public UserAccount GetByLocalCredentials(string username, string password)
        {
            password = HashHelper.ComputePassword(username, password);
            return Repository.FirstOrDefault(u => u.LocalCredentials != null && u.LocalCredentials.Username == username && u.LocalCredentials.Password == password && u.Enabled);
        }

        public UserAccount GetByRemoteCredentials(string username)
        {
            return Repository.FirstOrDefault(u => u.RemoteCredentials != null && u.RemoteCredentials.Username == username && u.Enabled);
        }
    }
}
