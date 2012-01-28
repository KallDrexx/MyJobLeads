﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ProcessParams.CompanySearching.Jigsaw;
using MyJobLeads.DomainModel.ViewModels.CompanySearching;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Entities.EF;
using RestSharp;
using MyJobLeads.DomainModel.Processes.ExternalAuth;
using System.Net;
using Newtonsoft.Json;
using MyJobLeads.DomainModel.Json.Jigsaw;
using AutoMapper;
using MyJobLeads.DomainModel.Exceptions.Jigsaw;
using MyJobLeads.DomainModel.Json.Converters;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Companies;

namespace MyJobLeads.DomainModel.Processes.CompanySearching
{
    public class JigsawCompanySearchProcesses 
        : IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>>,
          IProcess<JigsawCompanyDetailsParams, ExternalCompanyDetailsViewModel>,
          IProcess<MergeJigsawCompanyParams, GeneralSuccessResultViewModel>,
          IProcess<AddJigsawCompanyParams, CompanyAddedViewResult>
    {
        protected MyJobLeadsDbContext _context;
        protected CompanyByIdQuery _companyByIdQuery;
        protected CreateCompanyCommand _createCompanyCmd;

        public JigsawCompanySearchProcesses(MyJobLeadsDbContext context, CompanyByIdQuery companyQuery, CreateCompanyCommand createCompanyCmd)
        {
            _context = context;
            _companyByIdQuery = companyQuery;
            _createCompanyCmd = createCompanyCmd;
        }

        /// <summary>
        /// Performs a Jigsaw compan search
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public SearchResultsViewModel<ExternalCompanySearchResultViewModel> Execute(JigsawCompanySearchParams procParams)
        {
            const int PAGE_SIZE = 20;

            // perform the API query
            var client = new RestClient(@"http://www.jigsaw.com/");
            var request = new RestRequest("rest/searchCompany.json");
            request.AddParameter("token", JigsawAuthProcesses.GetAuthToken());
            request.AddParameter("name", procParams.CompanyName);
            request.AddParameter("industry", procParams.Industry);
            request.AddParameter("metroArea", procParams.MetroArea);
            request.AddParameter("state", procParams.State);
            request.AddParameter("employees", procParams.Employees);
            request.AddParameter("ownership", procParams.Ownership);
            request.AddParameter("order", "ASC");
            request.AddParameter("orderBy", "Company");
            request.AddParameter("offset", procParams.RequestedPageNum * PAGE_SIZE);
            request.AddParameter("pageSize", PAGE_SIZE);
            var response = client.Execute(request);

            // Check the result
            if (response.StatusCode != HttpStatusCode.OK)
                JigsawAuthProcesses.ThrowInvalidResponse(response.Content, procParams.RequestingUserId);

            // Convert the json to the view model
            var json = JsonConvert.DeserializeObject<CompanyDetailsResponseJson>(response.Content);
            var model = Mapper.Map<CompanyDetailsResponseJson, SearchResultsViewModel<ExternalCompanySearchResultViewModel>>(json);

            model.PageSize = PAGE_SIZE;
            model.DisplayedPageNumber = procParams.RequestedPageNum;

            return model;
        }

        /// <summary>
        /// Retrieves the detail of the specified company
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalCompanyDetailsViewModel Execute(JigsawCompanyDetailsParams procParams)
        {
            var json = GetJigsawCompany(procParams.JigsawId);
            return Mapper.Map<CompanyDetailsJson, ExternalCompanyDetailsViewModel>(json);
        }

        /// <summary>
        /// Merges the specified data from a jigsaw company to an existing MyLeads company
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(MergeJigsawCompanyParams procParams)
        {
            var company = _companyByIdQuery.RequestedByUserId(procParams.RequestingUserId).WithCompanyId(procParams.MyLeadsCompayId).Execute();
            if (company == null)
                throw new MJLEntityNotFoundException(typeof(Company), procParams.MyLeadsCompayId);

            var json = GetJigsawCompany(procParams.JigsawCompanyId);

            // Update the requested data
            if (procParams.MergeAddress)
            {
                company.City = json.city;
                company.State = json.state;
                company.Zip = json.zip;
            }

            if (procParams.MergeIndustry)
            {
                // Concat all the industries together
                string industries = string.Format("{0}: {1}, {2}: {3}, {4}: {5}",
                    json.industry1, json.subIndustry1, json.industry2, json.subIndustry2, json.industry3, json.subIndustry3);
                company.Industry = industries;
            }

            if (procParams.MergeName)
                company.Name = json.name;

            if (procParams.MergePhone)
                company.Phone = json.phone;

            // The rest of the merge fields are prepended into the company notes
            string notes = string.Empty;

            if (procParams.MergeOwnership)
                notes += string.Concat("Ownership: ", json.ownership, Environment.NewLine);

            if (procParams.MergeRevenue)
                notes += string.Concat("Revenue: ", json.revenue, Environment.NewLine);

            if (procParams.MergeWebsite)
                notes += string.Concat("Website: ", json.website, Environment.NewLine);

            if (procParams.MergeEmployeeCount)
                notes += string.Concat("Employees: ", json.employeeCount, Environment.NewLine);

            // Save the changes
            _context.SaveChanges();
            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }

        /// <summary>
        /// Adds a new company with data from the specified jigsaw company
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public CompanyAddedViewResult Execute(AddJigsawCompanyParams procParams)
        {
            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LastVisitedJobSearch)
                               .SingleOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            // Get the jigsaw details and create the company with those details
            var json = GetJigsawCompany(procParams.JigsawId);

            // Concat all the industries together
            string industries = string.Format("{0}: {1}, {2}: {3}, {4}: {5}",
                json.industry1, json.subIndustry1, json.industry2, json.subIndustry2, json.industry3, json.subIndustry3);

            // Form the notes
            string notes = string.Format("Ownership: {1}{0}Revenue: {2}{0}Website: {3}{0}Employees: {4}",
                Environment.NewLine, json.ownership, json.revenue, json.website, json.employeeCount);

            var company = _createCompanyCmd.CalledByUserId(procParams.RequestingUserId)
                                           .WithJobSearch((int)user.LastVisitedJobSearchId)
                                           .SetName(json.name)
                                           .SetPhone(json.phone)
                                           .SetCity(json.city)
                                           .SetState(json.state)
                                           .SetZip(json.zip)
                                           .SetIndustry(industries)
                                           .SetNotes(notes)
                                           .SetJigsawId(Convert.ToInt32(json.companyId))
                                           .Execute();

            return new CompanyAddedViewResult { CompanyId = company.Id, CompanyName = company.Name };
        }

        public static CompanyDetailsJson GetJigsawCompany(int id)
        {
            string companyUrl = string.Format("rest/companies/{0}.json", id);
            var client = new RestClient("https://www.jigsaw.com/");
            var request = new RestRequest(companyUrl, Method.GET);
            request.AddParameter("token", JigsawAuthProcesses.GetAuthToken());
            var response = client.Execute(request);

            var companyJson = JsonConvert.DeserializeObject<CompanyDetailsResponseJson>(response.Content, new JigsawDateTimeConverter());
            if (companyJson.companies.Count == 0)
                throw new JigsawCompanyNotFoundException(id.ToString());

            return companyJson.companies[0];
        }
    }
}
