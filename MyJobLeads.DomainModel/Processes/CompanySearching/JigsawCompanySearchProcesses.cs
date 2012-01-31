using System;
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
        protected EditCompanyCommand _editCompanyCmd;

        public JigsawCompanySearchProcesses(MyJobLeadsDbContext context, 
                                            CompanyByIdQuery companyQuery, 
                                            CreateCompanyCommand createCompanyCmd,
                                            EditCompanyCommand editCompanyCmd)
        {
            _context = context;
            _companyByIdQuery = companyQuery;
            _createCompanyCmd = createCompanyCmd;
            _editCompanyCmd = editCompanyCmd;
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
                JigsawAuthProcesses.ThrowInvalidResponse(response.Content, procParams.RequestingUserId, response.ErrorMessage);

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
            var json = GetJigsawCompany(procParams.JigsawId, procParams.RequestingUserId);
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

            var json = GetJigsawCompany(procParams.JigsawCompanyId, procParams.RequestingUserId);

            _editCompanyCmd.WithCompanyId(procParams.MyLeadsCompayId);
            _editCompanyCmd.RequestedByUserId(procParams.RequestingUserId);

            // Update the requested data
            if (procParams.MergeAddress)
            {
                _editCompanyCmd.SetCity(json.city);
                _editCompanyCmd.SetState(json.state);
                _editCompanyCmd.SetZip(json.zip);
            }

            if (procParams.MergeIndustry)
            {
                // Concat all the industries together
                string industries = string.Format("{0}: {1}, {2}: {3}, {4}: {5}",
                    json.industry1, json.subIndustry1, json.industry2, json.subIndustry2, json.industry3, json.subIndustry3);
                _editCompanyCmd.SetIndustry(industries);
            }

            if (procParams.MergeName)
                _editCompanyCmd.SetName(json.name);

            if (procParams.MergePhone)
                _editCompanyCmd.SetPhone(json.phone);

            // The rest of the merge fields are prepended into the company notes
            string notes = string.Empty;

            if (procParams.MergeWebsite)
                _editCompanyCmd.SetWebsite(json.website);

            if (procParams.MergeOwnership)
                notes += string.Concat("Ownership: ", json.ownership, Environment.NewLine);

            if (procParams.MergeRevenue)
                notes += string.Concat("Revenue: ", string.Format("${0:n0}", json.revenue), Environment.NewLine);

            if (procParams.MergeEmployeeCount)
                notes += string.Concat("Employees: ", string.Format("{0:n0}", json.employeeCount), Environment.NewLine);

            if (!string.IsNullOrWhiteSpace(notes))
                notes += Environment.NewLine;

            _editCompanyCmd.SetNotes(notes + company.Notes);

            // Save the changes
            _editCompanyCmd.Execute();
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
            var json = GetJigsawCompany(procParams.JigsawId, procParams.RequestingUserId);

            // Concat all the industries together
            string industries = string.Format("{0}: {1}, {2}: {3}, {4}: {5}",
                json.industry1, json.subIndustry1, json.industry2, json.subIndustry2, json.industry3, json.subIndustry3);

            // Form the notes
            string notes = string.Format("Ownership: {1}{0}Revenue: {2}{0}Employees: {3}",
                Environment.NewLine, json.ownership, string.Format("${0:n0}", json.revenue), string.Format("{0:n0}", json.employeeCount));

            var company = _createCompanyCmd.CalledByUserId(procParams.RequestingUserId)
                                           .WithJobSearch((int)user.LastVisitedJobSearchId)
                                           .SetName(json.name)
                                           .SetPhone(json.phone)
                                           .SetCity(json.city)
                                           .SetState(json.state)
                                           .SetZip(json.zip)
                                           .SetIndustry(industries)
                                           .SetNotes(notes)
                                           .SetWebsite(json.website)
                                           .SetJigsawId(Convert.ToInt32(json.companyId))
                                           .Execute();

            return new CompanyAddedViewResult { CompanyId = company.Id, CompanyName = company.Name };
        }

        public static CompanyDetailsJson GetJigsawCompany(int id, int requestingUserId)
        {
            string companyUrl = string.Format("rest/companies/{0}.json", id);
            var client = new RestClient("https://www.jigsaw.com/");
            var request = new RestRequest(companyUrl, Method.GET);
            request.AddParameter("token", JigsawAuthProcesses.GetAuthToken());
            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                JigsawAuthProcesses.ThrowInvalidResponse(response.Content, requestingUserId, response.ErrorMessage);

            var companyJson = JsonConvert.DeserializeObject<CompanyDetailsResponseJson>(response.Content, new JigsawDateTimeConverter());
            if (companyJson.companies == null || companyJson.companies.Count == 0)
                throw new JigsawCompanyNotFoundException(id.ToString());

            return companyJson.companies[0];
        }
    }
}
