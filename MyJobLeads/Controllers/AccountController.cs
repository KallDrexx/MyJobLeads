using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MyJobLeads.ViewModels;
using MyJobLeads.ViewModels.Accounts;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Queries.Organizations;

namespace MyJobLeads.Controllers
{
    public partial class AccountController : MyJobLeadsBaseController
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected IServiceFactory _serviceFactory;
        public AccountController(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            base.Initialize(requestContext);
        }

        public virtual ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public virtual ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public virtual ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.Email, model.Password, null);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.Email, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        public virtual ActionResult RegisterWithOrganization(Guid? registrationToken)
        {
            if (registrationToken == null)
                return RedirectToAction(MVC.Account.Register());

            // Retrieve the organization based on the token
            var org = _serviceFactory.GetService<OrganizationByRegistrationTokenQuery>()
                                     .Execute(new OrganizationByRegistrationTokenQueryParams { RegistrationToken = (Guid)registrationToken });
            if (org == null)
                return RedirectToAction(MVC.Account.Register());

            var model = new OrganizationRegistrationViewModel
            {
                RegistrationToken = (Guid)registrationToken,
                MinPasswordLength = Membership.MinRequiredPasswordLength,
                OrganizationName = org.Name
            };

            foreach (var orgDomain in org.EmailDomains)
                if (orgDomain.IsActive)
                    model.RestrictedEmailDomains.Add(orgDomain.Domain);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult RegisterWithOrganization(OrganizationRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;

                // Attempt to register the user
                try { createStatus = MembershipService.CreateUser(model.Email, model.Password, model.RegistrationToken); }
                catch (InvalidEmailDomainForOrganizationException)
                {
                    ModelState.AddModelError("", "The entered email address is not allowed for this organization");
                    createStatus = MembershipCreateStatus.ProviderError;
                }

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.Email, false /* createPersistentCookie */);
                    return RedirectToAction(MVC.Home.Index());
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form

            // Retrieve the organization based on the token
            var org = _serviceFactory.GetService<OrganizationByRegistrationTokenQuery>()
                                     .Execute(new OrganizationByRegistrationTokenQueryParams { RegistrationToken = model.RegistrationToken });

            foreach (var orgDomain in org.EmailDomains)
                if (orgDomain.IsActive)
                    model.RestrictedEmailDomains.Add(orgDomain.Domain);

            model.MinPasswordLength = Membership.MinRequiredPasswordLength;
            return View(model);
        }

        [Authorize]
        public virtual ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        public virtual ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public virtual ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult ResetPasswordResult(string userEmail)
        {
            var user = Membership.GetUser(userEmail);
            if (user == null)
                return View(false); // user wasn't found

            user.ResetPassword();
            return View(true);
        }
    }
}
