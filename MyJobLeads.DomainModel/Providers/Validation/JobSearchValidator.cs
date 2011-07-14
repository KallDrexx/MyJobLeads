using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Providers.Validation
{
    public class JobSearchValidator : AbstractValidator<JobSearch>
    {
        public JobSearchValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a name for the job search");
        }
    }
}
