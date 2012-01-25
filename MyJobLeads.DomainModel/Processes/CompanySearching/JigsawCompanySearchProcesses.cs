using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace MyJobLeads.DomainModel.Processes.CompanySearching
{
    public class JigsawCompanySearchProcesses 
        : IProcess<JigsawCompanySearchParams, SearchResultsViewModel<ExternalCompanySearchResultViewModel>>,
          IProcess<JigsawCompanyDetailsParams, ExternalCompanyDetailsViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public JigsawCompanySearchProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
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
            request.AddParameter("fortuneRank", procParams.FortuneRank);
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
