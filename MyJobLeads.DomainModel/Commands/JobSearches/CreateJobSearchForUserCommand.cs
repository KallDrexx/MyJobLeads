﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Queries.MilestoneConfigs;
using MyJobLeads.DomainModel.Providers;
using FluentValidation;
using MyJobLeads.DomainModel.Entities.EF;

namespace MyJobLeads.DomainModel.Commands.JobSearches
{
    /// <summary>
    /// Command to create a job search for a specific user
    /// </summary>
    public class CreateJobSearchForUserCommand
    {
        protected IServiceFactory _serviceFactory;
        protected int _userId;
        protected string _name, _description;

        public CreateJobSearchForUserCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Specifies the id value of the user to create the job search for
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CreateJobSearchForUserCommand ForUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the name of the new job search
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CreateJobSearchForUserCommand WithName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the description of the new job search
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public CreateJobSearchForUserCommand WithDescription(string description)
        {
            _description = description;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified user is not found</exception>
        public virtual JobSearch Execute()
        {
            var context = _serviceFactory.GetService<MyJobLeadsDbContext>();

            // Retrieve the user
            var user = context.Users.Where(x => x.Id == _userId).FirstOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the milestone this job search should start with
            //var milestone = _serviceFactory.GetService<StartingMilestoneQuery>()
            //                               .Execute(new StartingMilestoneQueryParams { OrganizationId = user.OrganizationId });

            // Create the job search
            var search = new JobSearch
            {
                Name = _name,
                Description = _description,
                User = user,

                Companies = new List<Company>(),
                History = new List<JobSearchHistory>()
            };

            // Create the history record
            search.History.Add(new JobSearchHistory
            {
                Name = _name,
                Description = _description,
                AuthoringUser = user,
                HistoryAction = MJLConstants.HistoryInsert,
                DateModified = DateTime.Now
            });

            // Perform validation
            var validator = _serviceFactory.GetService<IValidator<JobSearch>>();
            validator.ValidateAndThrow(search);

            context.JobSearches.Add(search);
            context.SaveChanges();

            return search;
        }
    }
}
