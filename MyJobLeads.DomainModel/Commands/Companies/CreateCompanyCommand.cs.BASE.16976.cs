using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Queries.JobSearches;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Queries.Users;
using MyJobLeads.DomainModel.Entities.History;
using MyJobLeads.DomainModel.Providers.Search;
using MyJobLeads.DomainModel.Providers;

namespace MyJobLeads.DomainModel.Commands.Companies
{
    /// <summary>
    /// Command that creates a new company
    /// </summary>
    public class CreateCompanyCommand
    {
        protected IServiceFactory _serviceFactory;
        protected int _searchId, _callingUserId;
        protected string _name, _phone, _city, _state, _zip, _metro, _industry, _notes;

        public CreateCompanyCommand(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        /// <summary>
        /// Specifies the id value of the job search the new company is associated with
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        public CreateCompanyCommand WithJobSearch(int jobSearchId)
        {
            _searchId = jobSearchId;
            return this;
        }

        /// <summary>
        /// Specifies the name for the company
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the phone number for the company
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetPhone(string phone)
        {
            _phone = phone;
            return this;
        }

        /// <summary>
        /// Specifies the city for the company
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetCity(string city)
        {
            _city = city;
            return this;
        }

        /// <summary>
        /// Specifies the company's state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetState(string state)
        {
            _state = state;
            return this;
        }

        /// <summary>
        /// Specifies the zip code for the company
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetZip(string zip)
        {
            _zip = zip;
            return this;
        }

        /// <summary>
        /// Specifies the company's metro area
        /// </summary>
        /// <param name="metroArea"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetMetroArea(string metroArea)
        {
            _metro = metroArea;
            return this;
        }

        /// <summary>
        /// Specifies the company's industry
        /// </summary>
        /// <param name="industry"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetIndustry(string industry)
        {
            _industry = industry;
            return this;
        }

        /// <summary>
        /// Specifies notes about the company
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public CreateCompanyCommand SetNotes(string notes)
        {
            _notes = notes;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user creating the company
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CreateCompanyCommand CalledByUserId(int userId)
        {
            _callingUserId = userId;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException"></exception>
        public virtual Company Execute()
        {
            // Retrieve the user creating this company
            
            var user = _serviceFactory.GetService<UserByIdQuery>().WithUserId(_callingUserId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _callingUserId);

            // Retrieve the job search
            var search = _serviceFactory.GetService<JobSearchByIdQuery>().WithJobSearchId(_searchId).Execute();
            if (search == null)
                throw new MJLEntityNotFoundException(typeof(JobSearch), _searchId);

            // Create the company entity
            var company = new Company
            {
                JobSearch = search,
                Name = _name,
                Phone = _phone,
                City = _city,
                State = _state,
                Zip = _zip,
                MetroArea = _metro,
                Industry = _industry,
                Notes = _notes,

                Tasks = new List<Task>(),
                Contacts = new List<Contact>(),
                History = new List<CompanyHistory>()
            };

            // Create the history record
            company.History.Add(new CompanyHistory
            {
                Name = _name,
                Phone = _phone,
                City = _city,
                State = _state,
                Zip = _zip,
                MetroArea = _metro,
                Industry = _industry,
                Notes = _notes,

                AuthoringUser = user,
                DateModified = DateTime.Now,
                HistoryAction = MJLConstants.HistoryInsert
            });

            var unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
            unitOfWork.Companies.Add(company);
            unitOfWork.Commit();

            // Index this new company for searching
            _serviceFactory.GetService<ISearchProvider>().Index(company);

            return company;
        }
    }
}
