﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Commands.Users;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.Infrastructure.Providers;
using MyJobLeads.App_Start;
using MyJobLeads.ViewModels.Accounts;

namespace MyJobLeads.Infrastructure
{
    public class MyJobLeadsMembershipProvider : MembershipProvider
    {
        #region Constructors

        public MyJobLeadsMembershipProvider() : this(null) { }

        public MyJobLeadsMembershipProvider(IServiceFactory factory)
        {
            // IF no factory was provided, we need to get one from the bootstrapper
            if (factory == null)
                _serviceFactory = new WindsorServiceFactory(Bootstrapper.WindsorContainer);
            else
                _serviceFactory = factory;
        }

        #endregion

        #region Implemented MembershipProvider Methods

        public override bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            var unitOfWork = GetUnitOfWork();

            // Retrieve the user with the specified email
            var user = new UserByEmailQuery(unitOfWork).WithEmail(email).Execute();
            if (user == null)
                return false;

            // Update the password
            try
            {
                new EditUserCommand(unitOfWork).WithUserId(user.Id)
                                                .WithExistingPassword(oldPassword)
                                                .SetPassword(newPassword)
                                                .Execute();
            }
            catch (MJLIncorrectPasswordException) { return false; }

            return true;
        }

        public MembershipUser CreateUser(CreateUserMembershipParams userParams, out MembershipCreateStatus status)
        {
            var unitOfWork = GetUnitOfWork();
            User user;

            try
            {
                user = new CreateUserCommand(_serviceFactory).Execute(new CreateUserCommandParams
                {
                    Email = userParams.Email,
                    PlainTextPassword = userParams.Password,
                    RegistrationToken = userParams.OrganizationRegistrationToken,
                    FullName = userParams.FullName
                });
            }
            catch (MJLDuplicateEmailException)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            status = MembershipCreateStatus.Success;
            return new MyJobLeadsMembershipUser(user); 
        }

        public override int MinRequiredPasswordLength
        {
            get { return PasswordUtils.MinPasswordLength; }
        }

        /// <summary>
        /// Determines if a user exists with the specified username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string email, string password)
        {
            var unitOfWork = GetUnitOfWork();
            var user = new UserByCredentialsQuery(unitOfWork).WithEmail(email)
                                                              .WithPassword(password)
                                                              .Execute();

            if (user == null)
                return false;
            else
                return true;
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            var unitOfWork = GetUnitOfWork();

            try 
            { 
                var user = new UserByEmailQuery(unitOfWork).WithEmail(email).Execute();
                if (user == null)
                    return null;

                return new MyJobLeadsMembershipUser(user);
            }
            catch (MJLUserNotFoundException)
            {
                return null;
            }
        }

        public override string ResetPassword(string email, string answer)
        {
            var unitOfWork = GetUnitOfWork();
            return new ResetUserPasswordCommand(unitOfWork).WithUserEmail(email).Execute();
        }

        #endregion

        #region Provider Utility Methods

        /// <summary>
        /// Retrieves the current unit of work from the system, to prevent object contexts from being kept around
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork GetUnitOfWork()
        {
           return _serviceFactory.GetService<IUnitOfWork>();
        }

        #endregion

        #region Member Variables

        protected IServiceFactory _serviceFactory;

        #endregion

        #region Non-Implemented MembershipProvider Methods

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                    bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}