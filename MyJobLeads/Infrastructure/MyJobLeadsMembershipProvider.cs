using System;
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

namespace MyJobLeads.Infrastructure
{
    public class MyJobLeadsMembershipProvider : MembershipProvider
    {
        #region Constructors

        public MyJobLeadsMembershipProvider() : this(null) { }

        public MyJobLeadsMembershipProvider(IUnitOfWork unitOfWork)
        {
            // If no unit of work was specified, create a default one
            if (unitOfWork == null)
                _unitOfWork = new EFUnitOfWork();
            else
                _unitOfWork = unitOfWork;
        }

        #endregion

        #region Implemented MembershipProvider Methods

        public override bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            // Retrieve the user with the specified email
            var user = new UserByEmailQuery(_unitOfWork).WithEmail(email).Execute();
            if (user == null)
                return false;

            // Update the password
            try
            {
                new EditUserCommand(_unitOfWork).WithUserId(user.Id)
                                                .WithExistingPassword(oldPassword)
                                                .SetPassword(newPassword)
                                                .Execute();
            }
            catch (MJLIncorrectPasswordException) { return false; }

            return true;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                    bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            User user;

            try
            {
                user = new CreateUserCommand(_unitOfWork).SetPassword(password)
                                                             .SetEmail(email)
                                                             .Execute();
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
            var user = new UserByCredentialsQuery(_unitOfWork).WithEmail(email)
                                                              .WithPassword(password)
                                                              .Execute();

            if (user == null)
                return false;
            else
                return true;
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            try 
            { 
                var user = new UserByEmailQuery(_unitOfWork).WithEmail(email).Execute();
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
            return new ResetUserPasswordCommand(_unitOfWork).WithUserEmail(email).Execute();
        }

        #endregion

        #region Member Variables

        protected IUnitOfWork _unitOfWork;

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