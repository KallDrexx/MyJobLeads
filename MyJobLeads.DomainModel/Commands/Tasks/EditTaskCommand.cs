using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.Tasks;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Queries.Contacts;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Commands.JobSearches;
using FluentValidation;

namespace MyJobLeads.DomainModel.Commands.Tasks
{
    public struct EditTaskCommandParams
    {
        public int TaskId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public bool? Completed { get; set; }
        public int RequestingUserId { get; set; }
        public string Notes { get; set; }

        private DateTime? _taskDate;
        public DateTime? TaskDate
        {
            get { return _taskDate; }
            set
            {
                _taskDate = value;
                TaskDateSet = true;
            }
        }

        public bool TaskDateSet { get; private set; }

        private int _contactId;
        public int ContactId
        {
            get { return _contactId; }
            set
            {
                _contactId = value;
                ContactSet = true;
            }
        }

        public bool ContactSet { get; private set; }
    }

    /// <summary>
    /// Command class that edits the specified task
    /// </summary>
    public class EditTaskCommand
    {
        protected IServiceFactory _serviceFactory;

        public EditTaskCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the specified task, calling user, or contact isn't found</exception>
        public virtual Task Execute(EditTaskCommandParams cmdParams)
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the user editing the task
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(cmdParams.RequestingUserId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), cmdParams.RequestingUserId);

            // Retrieve the task
            var task = _serviceFactory.GetService<TaskByIdQuery>().WithTaskId(cmdParams.TaskId).RequestedByUserId(cmdParams.RequestingUserId).Execute();
            if (task == null)
                throw new MJLEntityNotFoundException(typeof(Task), cmdParams.TaskId);

            // If specified, retrieve the specified contact
            Contact contact = null;
            if (cmdParams.ContactSet)
            {
                if (cmdParams.ContactId != 0)
                {
                    contact = _serviceFactory.GetService<ContactByIdQuery>()
                                             .WithContactId(cmdParams.ContactId)
                                             .RequestedByUserId(cmdParams.RequestingUserId)
                                             .Execute();
                    if (contact == null)
                        throw new MJLEntityNotFoundException(typeof(Contact), cmdParams.ContactId);
                }
            }

            // Only change the properties that were specified
            if (cmdParams.Name != null) { task.Name = cmdParams.Name; }
            if (cmdParams.Category != null) { task.Category = cmdParams.Category; }
            if (cmdParams.TaskDateSet) { task.TaskDate = cmdParams.TaskDate; }
            if (cmdParams.Notes != null) { task.Notes = cmdParams.Notes; }

            if (cmdParams.Completed != null) 
            {
                if (cmdParams.Completed.Value)
                {
                    // Only change the completion date if the completion date isn't already set
                    if (task.CompletionDate == null)
                        task.CompletionDate = DateTime.Now;
                }
                else
                    task.CompletionDate = null;
            }

            if (cmdParams.ContactSet) 
            {
                // ContactId must be set instead of Contact due to the contact not being eager loaded.
                task.ContactId = contact == null ? (int?)null : contact.Id;
            }

            // Create the history record
            task.History.Add(new TaskHistory
            {
                Name = task.Name,
                TaskDate = task.TaskDate,
                CompletionDate = task.CompletionDate,
                ContactId = task.ContactId,
                Category = task.Category,
                Notes = task.Notes,

                AuthoringUser = user,
                DateModified = DateTime.Now,
                HistoryAction = MJLConstants.HistoryUpdate
            });

            // Perform Validation
            var validation = _serviceFactory.GetService<IValidator<Task>>();
            validation.ValidateAndThrow(task);

            // Save
            unitOfWork.Commit();

            // Update the index for the task
            _serviceFactory.GetService<ISearchProvider>().Index(task);

            // Update the metrics for the task's job search
            var metricsCmd = _serviceFactory.GetService<UpdateJobSearchMetricsCommand>();
            metricsCmd.Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = (int)task.Company.JobSearchID });

            return task;
        }
    }
}
