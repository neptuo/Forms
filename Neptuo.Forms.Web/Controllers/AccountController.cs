using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.Messaging;
using Microsoft.Practices.Unity;
using Neptuo.Web.Mvc.Controllers;
using Neptuo.Web.Mvc.Html;
using Neptuo.Forms.Core.Service;
using Neptuo.Forms.Web.Models;

namespace Neptuo.Forms.Web.Controllers
{
    public class AccountController : AuthController<LocalLoginModel>
    {
        private const string OpenIDTextBox = "openid_identifier";
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        [Dependency]
        public IRemoteAuthProvider RemoteAuthProvider { get; set; }

        [Dependency]
        public IUserService Service { get; set; }

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
                        ShowMessage("Error using remote login. Use local credentials or try again later.", HtmlMessageType.Warning);
                        return View("login", new LocalLoginModel());
                    }
                }

                ShowMessage("Invalid identifier.", HtmlMessageType.Error);
                return View("login", new LocalLoginModel());
            }

            //Let us check the response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    if (RemoteAuthProvider.Authenticate(response.ClaimedIdentifier, false))
                    {
                        return AfterSuccesfulLogin(new LocalLoginModel
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
                UserCreateStatus status = Service.CreateAccount(model.Username, model.Fullname, model.Email);
                switch (status)
                {
                    case UserCreateStatus.Created:
                        ShowMessage((L)"User account created, please login once again.");
                        return RedirectToAction("login");
                    case UserCreateStatus.UsernameUsed:
                        ModelState.AddModelError("Username", (L)"Username already used!");
                        return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterLocal(LocalRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserCreateStatus status = Service.CreateAccount(model.Username, model.Password, model.Fullname, model.Email);
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
    }
}
