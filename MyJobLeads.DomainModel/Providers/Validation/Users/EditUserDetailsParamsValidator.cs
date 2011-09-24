using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.ProcessParams.Users;
using FluentValidation;

namespace MyJobLeads.DomainModel.Providers.Validation.Users
{
    public class EditUserDetailsParamsValidator : AbstractValidator<EditUserDetailsParams>
    {
        public EditUserDetailsParamsValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("A valid full name is required");
            RuleFor(x => x.NewPasswordConfirmation).Equal(x => x.NewPassword).WithMessage("New password and the new password confirmation do not match");
            RuleFor(x => x.NewEmail).EmailAddress().WithMessage("A valid email is required");
        }
    }
}
