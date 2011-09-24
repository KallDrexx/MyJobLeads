using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.ProcessParams.Users;
using MyJobLeads.DomainModel.Data;
using FluentValidation;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Utilities;
using FluentValidation.Results;

namespace MyJobLeads.DomainModel.Processes.Users
{
    public class UserProcesses : IProcess<EditUserDetailsParams, bool>
    {
        public UserProcesses(UserByCredentialsQuery userCredentialsQuery, 
                            UserByEmailQuery userEmailQuery,
                            IValidator<EditUserDetailsParams> editUserDetailsValidator,
                            IUnitOfWork unitOfWork) 
        {
            _userCredentialsQuery = userCredentialsQuery;
            _editUserDetailsValidator = editUserDetailsValidator;
            _unitOfWork = unitOfWork;
            _userEmailQuery = userEmailQuery;
        }

        /// <summary>
        /// Edits the user details
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public bool Execute(EditUserDetailsParams procParams)
        {
            // Validate parameters
            var validationResults = _editUserDetailsValidator.Validate(procParams);
            var currentUser = _userCredentialsQuery.WithEmail(procParams.CurrentEmail).WithPassword(procParams.CurrentPassword).Execute();
            var userWithEmail = _userEmailQuery.WithEmail(procParams.NewEmail).Execute();
            if (currentUser == null)
                validationResults.Errors.Add(new ValidationFailure("", "Current password was not correct"));

            if (userWithEmail != null && currentUser == userWithEmail)
                validationResults.Errors.Add(new ValidationFailure("", "A user with the specified email address already exists"));

            if (!validationResults.IsValid)
                throw new ValidationException(validationResults.Errors);

            // Update details
            currentUser.Email = procParams.NewEmail.ToLower().Trim();
            currentUser.FullName = procParams.FullName.Trim();

            // Update the password, either from the old password or new one if specified
            if (!string.IsNullOrWhiteSpace(procParams.NewPassword))
                currentUser.Password = PasswordUtils.CreatePasswordHash(currentUser.Email, procParams.NewPassword);
            else
                currentUser.Password = PasswordUtils.CreatePasswordHash(currentUser.Email, procParams.CurrentPassword);

            _unitOfWork.Commit();

            return true;
        }

        #region Member Variables

        protected UserByCredentialsQuery _userCredentialsQuery;
        protected UserByEmailQuery _userEmailQuery;
        protected IValidator<EditUserDetailsParams> _editUserDetailsValidator;
        protected IUnitOfWork _unitOfWork;

        #endregion
    }
}
