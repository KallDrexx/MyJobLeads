using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using FluentValidation;

namespace MyJobLeads.DomainModel.Providers.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Entered email is not a valid email address");
        }
    }
}
