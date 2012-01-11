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
using MyJobLeads.DomainModel.Queries.Users;
using AutoMapper;
using MyJobLeads.DomainModel.ProcessParams.Users;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Data;
using FluentValidation;

namespace MyJobLeads.Controllers
{
    public partial class AccountController : MyJobLeadsBaseController
    {
        protected EditUserCommand _editUserCmd;
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public AccountController(IServiceFactory factory, EditUserCommand editUserCmd)
        {
            _serviceFactory = factory;
            _editUserCmd = editUserCmd;


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
            return View(MVC.Account.Views.ClosedRegistrationView);
        }

        [HttpPost]
        public virtual ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var createStatus = MembershipService.CreateUser(new CreateUserMembershipParams
                {
                    Email = model.Email,
                    Password = model.Password,
                    FullName = model.FullName
                });

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
                return View(MVC.Account.Views.InvalidOrgRegistrationTokenView);

            // Retrieve the organization based on the token
            var org = _serviceFactory.GetService<OrganizationByRegistrationTokenQuery>()
                                     .Execute(new OrganizationByRegistrationTokenQueryParams { RegistrationToken = (Guid)registrationToken });
            if (org == null)
                return View(MVC.Account.Views.InvalidOrgRegistrationTokenView);

            var model = new OrganizationRegistrationViewModel
            {
                RegistrationToken = (Guid)registrationToken,
                MinPasswordLength = Membership.MinRequiredPasswordLength,
                OrganizationName = org.Name
            };

            if (org.EmailDomains.Count > 0)
                model.RestrictedEmailDomain = org.EmailDomains.First().Domain;

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult RegisterWithOrganization(OrganizationRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;

                // Attempt to register the user
                try 
                {
                    createStatus = MembershipService.CreateUser(new CreateUserMembershipParams
                    {
                        Email = model.Email,
                        Password = model.Password,
                        OrganizationRegistrationToken = model.RegistrationToken,
                        FullName = model.FullName
                    });
                    
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
                catch (InvalidEmailDomainForOrganizationException)
                {
                    ModelState.AddModelError("", "The entered email address is not allowed for this organization");
                }
            }

            // If we got this far, something failed, redisplay form

            // Retrieve the organization based on the token
            var org = _serviceFactory.GetService<OrganizationByRegistrationTokenQuery>()
                                     .Execute(new OrganizationByRegistrationTokenQueryParams { RegistrationToken = model.RegistrationToken });

            if (org.EmailDomains.Count > 0)
                model.RestrictedEmailDomain = org.EmailDomains.First().Domain;

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
        public virtual ActionResult ResetPassword(string userEmail)
        {
            var user = Membership.GetUser(userEmail);
            if (user == null)
            {
                ModelState.AddModelError("userEmail", string.Format("The email {0} is not an email for a current MyLeads account", userEmail));
                ViewBag.LastEmail = userEmail;
                return View(MVC.Account.Views.ResetPassword); // user wasn't found
            }

            user.ResetPassword();
            return RedirectToAction(MVC.Account.ResetPasswordResult());
        }

        public virtual ActionResult ResetPasswordResult()
        {
            return View();
        }

        public virtual ActionResult Edit()
        {
            // If the user isn't logged in, redirect to home
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(MVC.Home.Index());

            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(CurrentUserId).Execute();
            var model = Mapper.Map<User, EditUserDetailsParams>(user);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(EditUserDetailsParams model)
        {
            if (model.NewPassword != model.NewPasswordConfirmation)
                ModelState.AddModelError("", "Your new password and the new password confirmation do not match");

            if (string.IsNullOrWhiteSpace(model.FullName))
                ModelState.AddModelError("FullName", "You must enter a valid full name");

            try
            {
                _editUserCmd.WithUserId(CurrentUserId)
                            .WithExistingPassword(model.CurrentPassword)
                            .SetPassword(model.NewPassword)
                            .SetFullName(model.FullName)
                            .Execute();
            }

            catch (MJLDuplicateEmailException)
            {
                ModelState.AddModelError("NewEmail", "A user already exists with the specified email address");
            }

            catch (MJLIncorrectPasswordException)
            {
                ModelState.AddModelError("CurrentPassword", "Your current password is incorrect");
            }

            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction(MVC.Home.Index());
        }
    }
}
