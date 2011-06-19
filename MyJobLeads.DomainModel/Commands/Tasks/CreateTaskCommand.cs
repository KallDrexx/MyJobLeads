using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Commands.JobSearches;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    public struct CreateTaskCommandParams
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime? TaskDate { get; set; }
        public int RequestedUserId { get; set; }
        public int ContactId { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Command class to create a new task for a company
    /// </summary>
    public class CreateTaskCommand
    {
        protected IServiceFactory _serviceFactory;

        public CreateTaskCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified company, calling user, or contact is not found</exception>
        public virtual Task Execute(CreateTaskCommandParams cmdParams)
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the user creating the task
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(cmdParams.RequestedUserId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), cmdParams.RequestedUserId);

            // Retrieve the company
            var company = _serviceFactory.GetService<CompanyByIdQuery>().WithCompanyId(cmdParams.CompanyId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), cmdParams.CompanyId);

            // Retrieve the contact if one is specified
            Contact contact = null;
            if (cmdParams.ContactId != 0)
            {
                contact = _serviceFactory.GetService<ContactByIdQuery>().WithContactId(cmdParams.ContactId).Execute();
                if (contact == null)
                    throw new MJLEntityNotFoundException(typeof(Contact), cmdParams.ContactId);
            }

            // Create the Task
            var task = new Task
            {
                Company = company,
                Name = cmdParams.Name,
                TaskDate = cmdParams.TaskDate,
                Contact = contact,
                Category = cmdParams.Category,
                Notes = cmdParams.Notes,

                History = new List<TaskHistory>()
            };

            // Create history record
            task.History.Add(new TaskHistory
            {
                Name = cmdParams.Name,
                TaskDate = cmdParams.TaskDate,
                Category = cmdParams.Category,
                Notes = cmdParams.Notes,
                HistoryAction = MJLConstants.HistoryInsert,
                DateModified = DateTime.Now,
                AuthoringUser = user
            });

            unitOfWork.Tasks.Add(task);
            unitOfWork.Commit();

            // Index the task with the search provider
            _serviceFactory.GetService<ISearchProvider>().Index(task);

            // Update the metrics for the job search
            _serviceFactory.GetService<UpdateJobSearchMetricsCommand>()
                .Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = (int)company.JobSearchID });

            return task;
        }
    }
}
