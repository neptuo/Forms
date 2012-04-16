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
        /// <summary>
        /// Creates user account with local creadentials.
        /// </summary>
        /// <param name="username">Local username.</param>
        /// <param name="password">Local password.</param>
        /// <param name="fullname">User fullname.</param>
        /// <param name="email">User e-mail address.</param>
        /// <returns>Creation status, <see cref="UserCreateStatus"/>.</returns>
        UserCreateStatus CreateAccount(string username, string password, string fullname, string email);

        /// <summary>
        /// Creates user account with remote credentials.
        /// </summary>
        /// <param name="remoteUsername">Remote username/claimed identifier.</param>
        /// <param name="fullname">User fullname.</param>
        /// <param name="email">User e-mail address.</param>
        /// <returns>Creation status, <see cref="UserCreateStatus"/>.</returns>
        UserCreateStatus CreateAccount(string remoteUsername, string fullname, string email);

        /// <summary>
        /// Updates current user account.
        /// </summary>
        /// <param name="fullname">New user fullname.</param>
        /// <param name="email">New user e-mail address.</param>
        /// <returns>Update account status, <see cref="UserUpdateStatus"/>.</returns>
        UserUpdateStatus UpdateAccount(string fullname, string email);

        /// <summary>
        /// Changes password of current user.
        /// </summary>
        /// <param name="currentPassword">Current account password.</param>
        /// <param name="newPassword">New account password.</param>
        /// <returns>Change password status, <see cref="ChangePasswordStatus"/>.</returns>
        ChangePasswordStatus ChangePassword(string currentPassword, string newPassword);

        /// <summary>
        /// Disables user account.
        /// </summary>
        /// <param name="id">ID of account to disable.</param>
        void DisableUser(int id);

        /// <summary>
        /// Enables user account.
        /// </summary>
        /// <param name="id">ID of account to enable.</param>
        void EnableUser(int id);

        /// <summary>
        /// Makes user system admin.
        /// </summary>
        /// <param name="id">ID of account to make admin.</param>
        void MakeAdmin(int id);

        /// <summary>
        /// Makes user as normal registered user.
        /// </summary>
        /// <param name="id">ID of account to make user.</param>
        void MakeUser(int id);

        /// <summary>
        /// Gets account by ID.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>User account with <paramref name="id"/>.</returns>
        UserAccount Get(int id);

        /// <summary>
        /// Gets account by username (local or remote).
        /// </summary>
        /// <param name="username">Account username/claimed identifier.</param>
        /// <returns>User account with <paramref name="username"/>.</returns>
        UserAccount Get(string username);

        /// <summary>
        /// Gets list of all user accounts.
        /// </summary>
        /// <returns>All of accounts.</returns>
        IQueryable<UserAccount> GetList();

        /// <summary>
        /// Gets user account by local credentials (<paramref name="username"/> and <paramref name="password"/>).
        /// </summary>
        /// <param name="username">Local username.</param>
        /// <param name="password">Local password.</param>
        /// <returns>User account associated with <paramref name="username"/> and <paramref name="password"/>.</returns>
        UserAccount GetByLocalCredentials(string username, string password);

        /// <summary>
        /// Gets user account by remote username/claimed identifier (<paramref name="username"/>).
        /// </summary>
        /// <param name="username">Remote username/claimed identifier.</param>
        /// <returns>User account associated with <paramref name="username"/>.</returns>
        UserAccount GetByRemoteCredentials(string username);
    }

    /// <summary>
    /// Statuses that can occur while registering user account.
    /// </summary>
    public enum UserCreateStatus
    {
        Created, UsernameUsed, InsufficientPassword
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
