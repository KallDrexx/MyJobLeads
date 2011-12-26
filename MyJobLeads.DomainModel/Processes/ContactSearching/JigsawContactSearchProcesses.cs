using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.ExternalAuth;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuthorization.Jigsaw;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.Exceptions.Jigsaw;
using RestSharp;
using MyJobLeads.DomainModel.Processes.ExternalAuth;
using System.Net;
using Newtonsoft.Json;
using MyJobLeads.DomainModel.Json.Jigsaw;
using AutoMapper;

namespace MyJobLeads.DomainModel.Processes.ContactSearching
{
    public class JigsawContactSearchProcesses : IProcess<JigsawContactSearchParams, ExternalContactSearchResultsViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<GetUserJigsawCredentialsParams, JigsawCredentialsViewModel> _getJigsawCredentialsProc;

        public JigsawContactSearchProcesses(MyJobLeadsDbContext context, IProcess<GetUserJigsawCredentialsParams, JigsawCredentialsViewModel> getJsCredProc)
        {
            _context = context;
            _getJigsawCredentialsProc = getJsCredProc;
        }

        /// <summary>
        /// Performs a jigsaw search on the user's behalf
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalContactSearchResultsViewModel Execute(JigsawContactSearchParams procParams)
        {
            const int PAGE_SIZE = 100;

            var credentials = _getJigsawCredentialsProc.Execute(new GetUserJigsawCredentialsParams { RequestingUserId = procParams.RequestingUserId });
            if (credentials == null)
                throw new JigsawCredentialsNotFoundException(procParams.RequestingUserId);

            // Perform the API query
            var client = new RestClient("https://www.jigsaw.com/");
            var request = new RestRequest("rest/searchContact.json", Method.GET);
            request.AddParameter("token", JigsawAuthProcesses.GetAuthToken());
            request.AddParameter("username", credentials.JigsawUsername);
            request.AddParameter("password", credentials.JigsawPassword);
            request.AddParameter("firstname", procParams.FirstName);
            request.AddParameter("lastname", procParams.LastName);
            request.AddParameter("levels", procParams.Levels);
            request.AddParameter("companyName", procParams.CompanyName);
            request.AddParameter("title", procParams.Title);
            request.AddParameter("industry", procParams.Industry);
            request.AddParameter("departments", procParams.Departments);
            request.AddParameter("offset", procParams.RequestedPageNum * PAGE_SIZE);
            request.AddParameter("pagesize", PAGE_SIZE);
            var response = client.Execute(request);

            // Check for a forbidden result
            if (response.StatusCode == HttpStatusCode.Forbidden)
                JigsawAuthProcesses.ThrowForbiddenResponse(response.Content, procParams.RequestingUserId);

            // Get the returned json and convert it to the view model
            var json = JsonConvert.DeserializeObject<ContactSearchResponseJson>(response.Content);
            var model = Mapper.Map<ContactSearchResponseJson, ExternalContactSearchResultsViewModel>(json);
            model.PageSize = PAGE_SIZE;
            model.DisplayedPageNumber = procParams.RequestedPageNum;

            return model;
        }
    }
}
