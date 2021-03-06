﻿using System;
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
using FluentValidation;

namespace MyJobLeads.DomainModel.Commands.Companies
{
    public class EditCompanyCommand
    {
        protected IServiceFactory _factory;
        protected int _companyId, _userId;
        protected string _name, _phone, _city, _state, _zip, _metro, _industry, _notes;
        protected string _status, _site;

        public EditCompanyCommand(IServiceFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Specifies the id value of the company to edit
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        public EditCompanyCommand WithCompanyId(int companyId)
        {
            _companyId = companyId;
            return this;
        }

        /// <summary>
        /// Specifies the name for the company
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EditCompanyCommand SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Specifies the phone number for the company
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public EditCompanyCommand SetPhone(string phone)
        {
            _phone = phone;
            return this;
        }

        /// <summary>
        /// Specifies the city for the company
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public EditCompanyCommand SetCity(string city)
        {
            _city = city;
            return this;
        }

        /// <summary>
        /// Specifies the company's state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public EditCompanyCommand SetState(string state)
        {
            _state = state;
            return this;
        }

        /// <summary>
        /// Specifies the zip code for the company
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public EditCompanyCommand SetZip(string zip)
        {
            _zip = zip;
            return this;
        }

        /// <summary>
        /// Specifies the company's metro area
        /// </summary>
        /// <param name="metroArea"></param>
        /// <returns></returns>
        public EditCompanyCommand SetMetroArea(string metroArea)
        {
            _metro = metroArea;
            return this;
        }

        /// <summary>
        /// Specifies the company's industry
        /// </summary>
        /// <param name="industry"></param>
        /// <returns></returns>
        public EditCompanyCommand SetIndustry(string industry)
        {
            _industry = industry;
            return this;
        }

        /// <summary>
        /// Specifies notes about the company
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public EditCompanyCommand SetNotes(string notes)
        {
            _notes = notes;
            return this;
        }

        /// <summary>
        /// Specifies the website for the company
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public EditCompanyCommand SetWebsite(string site)
        {
            _site = site;
            return this;
        }

        /// <summary>
        /// Specifies the id value of the user editing the company
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EditCompanyCommand RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Specifies the lead status to give to the company
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public EditCompanyCommand SetLeadStatus(string status)
        {
            _status = status;
            return this;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MJLEntityNotFoundException">Thrown when the company or requesting user doesn't exist</exception>
        public virtual Company Execute()
        {
            var unitOfWork = _factory.GetService<IUnitOfWork>();

            // Retrieve the calling user
            var user = _factory.GetService<UserByIdQuery>().WithUserId(_userId).Execute();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), _userId);

            // Retrieve the entity
            var company = _factory.GetService<CompanyByIdQuery>().WithCompanyId(_companyId).RequestedByUserId(_userId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), _companyId);

            // Edit only the properties that have been changed (not null)
            if (_name != null) { company.Name = _name; }
            if (_phone != null) { company.Phone = _phone; }
            if (_city != null) { company.City = _city; }
            if (_state != null) { company.State = _state; }
            if (_zip != null) { company.Zip = _zip; }
            if (_metro != null) { company.MetroArea = _metro; }
            if (_industry != null) { company.Industry = _industry; }
            if (_notes != null) { company.Notes = _notes; }
            if (_status != null) { company.LeadStatus = _status; }
            if (_site != null) { company.Website = _site; }

            // Create the history record
            company.History.Add(new CompanyHistory
            {
                Name = company.Name,
                Phone = company.Phone,
                City = company.City,
                State = company.State,
                Zip = company.Zip,
                MetroArea = company.MetroArea,
                Industry = company.Industry,
                Notes = company.Notes,
                LeadStatus = company.LeadStatus,
                JigsawId = company.JigsawId,
                Website = company.Website,

                AuthoringUser = user,
                HistoryAction = MJLConstants.HistoryUpdate,
                DateModified = DateTime.Now
            });

            // Perform validation
            var validator = _factory.GetService<IValidator<Company>>();
            validator.ValidateAndThrow(company);

            // Commit changes
            unitOfWork.Commit();

            // Index the edited company
            _factory.GetService<ISearchProvider>().Index(company);

            return company;
        }
    }
}
