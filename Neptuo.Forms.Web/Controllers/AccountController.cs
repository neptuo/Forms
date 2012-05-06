using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.Messaging;
using Microsoft.Practices.Unity;
using Neptuo.Web.Mvc.Controllers;
using Neptuo.Web.Mvc.Html;
using Neptuo.Forms.Core;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models;

namespace Neptuo.Forms.Web.Controllers
{
    public class AccountController : AuthController<LocalLoginModel>
    {
        public const string OpenIDTextBox = "openid_identifier";
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        [Dependency]
        public IActivityService ActivityService { get; set; }

        [Dependency]
        public UserContext UserContext { get; set; }

        [Dependency]
        public IUserService UserService { get; set; }

        [Dependency]
        public IInvitationService InvitationService { get; set; }

        [Dependency]
        public IRemoteAuthProvider RemoteAuthProvider { get; set; }

        #region Authentication

        protected override ActionResult AfterLoginFailure(LocalLoginModel model)
        {
            ActivityService.UserLoginFailure(model.Username);
            ShowMessage((L)"No such user account!", HtmlMessageType.Error);
            return base.AfterLoginFailure(model);
        }

        protected override ActionResult AfterSuccessfulLogin(LocalLoginModel model)
        {
            ActivityService.UserLoggedIn(model.Username);
            string message = (L)"Welcome back, {0}!";

            IQueryable<ProjectInvitation> invitations = InvitationService.GetProjectInvitations();
            if (invitations.Count() > 0)
                message = String.Format("{1} " + (L)"You have {0} invitation(s).", invitations.Count(), message);

            ShowMessage(String.Format(message, model.Username));
            return base.AfterSuccessfulLogin(model);
        }

        protected override ActionResult AfterSuccessfulLogout()
        {
            string username = null;
            if (UserContext.Account.LocalCredentials != null)
                username = UserContext.Account.LocalCredentials.Username;
            else
                username = UserContext.Account.RemoteCredentials.Username;

            ActivityService.UserLoggedOut(username);
            return base.AfterSuccessfulLogout();
        }

        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
            var response = openid.GetResponse();
            if (response == null)
            {
                Identifier id;
                if (Identifier.TryParse(Request.Form[OpenIDTextBox], out id))
                {
                    try
                    {
                        var request = openid.CreateRequest(Request.Form[OpenIDTextBox]);
                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException)
                    {
                        ShowMessage((L)"Error using remote login. Use local credentials or try again later.", HtmlMessageType.Warning);
                        return View("login", new LocalLoginModel());
                    }
                }

                ShowMessage((L)"Invalid identifier.", HtmlMessageType.Error);
                return View("login", new LocalLoginModel());
            }

            //Let us check the response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    if (RemoteAuthProvider.Authenticate(response.ClaimedIdentifier, false))
                    {
                        return AfterSuccessfulLogin(new LocalLoginModel
                        {
                            Username = response.ClaimedIdentifier
                        });
                    }
                    else
                    {
                        TempData["OpenID"] = response.ClaimedIdentifier;
                        return RedirectToAction("register");
                    }
                case AuthenticationStatus.Canceled:
                    ShowMessage((L)"Login request was canceled.", HtmlMessageType.Warning);
                    return View("login", new LocalLoginModel());
                case AuthenticationStatus.Failed:
                    ShowMessage((L)"Login request failed.", HtmlMessageType.Warning);
                    return View("login", new LocalLoginModel());
            }

            return new EmptyResult();
        }

        #endregion

        #region Registration

        public ActionResult Register()
        {
            if (TempData["OpenID"] != null)
            {
                return View(new RegisterModel
                {
                    Username = TempData["OpenID"].ToString()
                });
            }
            return View("RegisterLocal", new LocalRegisterModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserCreateStatus status = UserService.CreateAccount(model.Username, model.Fullname, model.Email);
                switch (status)
                {
                    case UserCreateStatus.Created:
                        ShowMessage((L)"User account created, please login once again.");
                        return RedirectToAction("login");
                    case UserCreateStatus.UsernameUsed:
                        ModelState.AddModelError("Username", (L)"Username already used!");
                        break;
                    case UserCreateStatus.InsufficientPassword:
                        ModelState.AddModelError("Password", (L)"Insufficient password complexity!");
                        break;
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterLocal(LocalRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserCreateStatus status = UserService.CreateAccount(model.Username, model.Password, model.Fullname, model.Email);
                switch (status)
                {
                    case UserCreateStatus.Created:
                        ShowMessage((L)"User account created.");
                        return RedirectToAction("login");
                    case UserCreateStatus.UsernameUsed:
                        ModelState.AddModelError("Username", (L)"Username already used!");
                        return View(model);
                }
            }
            return View(model);
        }

        #endregion

        #region Change/Change password

        [AuthorizeUser]
        public ActionResult Change()
        {
            return View("change", new ChangeAccountModel
            {
                Email = UserContext.Account.Email,
                Fullname = UserContext.Account.Fullname
            });
        }

        [AuthorizeUser]
        [HttpPost]
        public ActionResult Change(ChangeAccountModel model)
        {
            if (ModelState.IsValid)
            {
                UserUpdateStatus status = UserService.UpdateAccount(model.Fullname, model.Email);
                if (status == UserUpdateStatus.Updated)
                {
                    ShowMessage((L)"Account updated");
                    return Change();
                }
                else
                {
                    ShowMessage((L)"No such user account", HtmlMessageType.Error);
                }
            }
            return View(model);
        }

        [AuthorizeUser]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                ChangePasswordStatus status =  UserService.ChangePassword(model.CurrentPassword, model.Password);
                switch (status)
                {
                    case ChangePasswordStatus.Changed:
                        ShowMessage((L)"Password changed.");
                        return RedirectToAction("change");
                    case ChangePasswordStatus.InvalidCurrentPassword:
                        ModelState.AddModelError("CurrentPassword", (L)"Invalid current password!");
                        break;
                    case ChangePasswordStatus.InsuficientComplexity:
                        ModelState.AddModelError("Password", (L)"Insuficient password complexity!");
                        break;
                    case ChangePasswordStatus.NoSuchUser:
                        ShowMessage((L)"No such user account", HtmlMessageType.Error);
                        break;
                    case ChangePasswordStatus.NoLocalCredentials:
                        ShowMessage((L)"You don't have local credentials to manage!");
                        break;
                }
            }
            return Change();
        }

        #endregion

        #region Settings

        [AuthorizeUser]
        public ActionResult Settings()
        {
            return View();
        }

        #endregion
    }
}
