using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Provides access to user accounts.
    /// </summary>
    public interface IUserService
    {
        UserCreateStatus CreateAccount(string username, string password, string fullname, string email);

        UserCreateStatus CreateAccount(string remoteUsername, string fullname, string email);

        UserUpdateStatus UpdateAccount(string fullname, string email);

        ChangePasswordStatus ChangePassword(string currentPassword, string newPassword);

        void DisableUser(int id);

        void EnableUser(int id);

        void MakeAdmin(int id);

        void MakeUser(int id);

        UserAccount Get(int id);

        UserAccount Get(string username);

        IQueryable<UserAccount> GetList();

        UserAccount GetByLocalCredentials(string username, string password);

        UserAccount GetByRemoteCredentials(string username);
    }

    /// <summary>
    /// Statuses that can occur while registering user account.
    /// </summary>
    public enum UserCreateStatus
    {
        Created, UsernameUsed
    }

    /// <summary>
    /// Statuses that can occur while updating user account.
    /// </summary>
    public enum UserUpdateStatus
    {
        Updated, NoSuchUser
    }

    /// <summary>
    /// Statuses that can occur while changing password.
    /// </summary>
    public enum ChangePasswordStatus
    {
        Changed, InvalidCurrentPassword, InsuficientComplexity, NoSuchUser, NoLocalCredentials
    }
}
