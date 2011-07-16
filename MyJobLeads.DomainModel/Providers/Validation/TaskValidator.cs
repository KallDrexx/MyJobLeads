using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;
using FluentValidation;

namespace MyJobLeads.DomainModel.Providers.Validation
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The task must have a subject");
        }
    }
}
