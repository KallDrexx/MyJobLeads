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
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Commands.JobSearches;
using FluentValidation;

namespace MyJobLeads.DomainModel.Commands.Contacts
{
    /// <summary>
    /// Command class for creating a new contact
    /// </summary>
    public class CreateContactCommand
    {
        protected IServiceFactory _serviceFactory;
        protected int _companyId, _userId;
        protected string _name, _directPhone, _mobilePhone, _ext, _email, _assistant, _referredBy, _notes;

        public CreateContactCommand(IServiceFactory factory)
        {
            _serviceFactory = factory;
        }

        /// <summary>
        /// Specifies the id value of the company to create the contact for
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public CreateContactCommand WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Specifies the name for the contact
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CreateContactCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the contact's direct phone number
        /// </summary>
        /// <param name="directPhone"></param>
        /// <returns></returns>
        public CreateContactCommand SetDirectPhone(string directPhone)
        {
            _directPhone = directPhone;
            return this;
        }

        /// <summary>
        /// Specifies the contact's mobile phone number
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public CreateContactCommand SetMobilePhone(string mobilePhone)
        {
            _mobilePhone = mobilePhone;
            return this;
        }

        /// <summary>
        /// Specifies the contact's extension
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public CreateContactCommand SetExtension(string extension)
        {
            _ext = extension;
            return this;
        }

        /// <summary>
        /// Specifies the contact's email
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public CreateContactCommand SetEmail(string email)
        {
            _email = email;
            return this;
        }

        /// <summary>
        /// Specifies the name of the contact's assistant
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public CreateContactCommand SetAssistant(string assistant)
        {
            _assistant = assistant;
            return this;
        }

        /// <summary>
        /// Specifies who the contact was referred by
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public CreateContactCommand SetReferredBy(string referredBy)
        {
            _referredBy = referredBy;
            return this;
        }

        /// <summary>
        /// Specifies the notes for the contact
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public CreateContactCommand SetNotes(string notes)
        {
            _notes = notes;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user creating the contact
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CreateContactCommand RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the company or calling user is not found</exception>
        public virtual Contact Execute()
        {
            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            // Retrieve the user creating the contact
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the company
            var company = _serviceFactory.GetService<CompanyByIdQuery>().WithCompanyId(_companyId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), _companyId);

            // Create the contact
            var contact = new Contact
            {
                Company = company,
                Name = _name,
                DirectPhone = _directPhone,
                MobilePhone = _mobilePhone,
                Extension = _ext,
                Email = _email,
                Assistant = _assistant,
                ReferredBy = _referredBy,
                Notes = _notes,

                Tasks = new List<Task>(),
                History = new List<ContactHistory>()
            };

            // Create the history record
            contact.History.Add(new ContactHistory
            {
                Name = _name,
                DirectPhone = _directPhone,
                MobilePhone = _mobilePhone,
                Extension = _ext,
                Email = _email,
                Assistant = _assistant,
                ReferredBy = _referredBy,
                Notes = _notes,

                DateModified = DateTime.Now,
                AuthoringUser = user,
                HistoryAction = MJLConstants.HistoryInsert
            });

            // Perform Validation
            var validator = _serviceFactory.GetService<IValidator<Contact>>();
            validator.ValidateAndThrow(contact);

            // Commit to database
            unitOfWork.Contacts.Add(contact);
            unitOfWork.Commit();

            // Index the new contact so it can be searched
            _serviceFactory.GetService<ISearchProvider>().Index(contact);

            // Update the job search metrics for the contact
            var metricsCmd = _serviceFactory.GetService<UpdateJobSearchMetricsCommand>();
            metricsCmd.Execute(new UpdateJobSearchMetricsCmdParams { JobSearchId = (int)company.JobSearchID });

            return contact;
        }
    }
}