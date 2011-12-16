using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Entity;
using System.Web;
using MyJobLeads.DomainModel.Processes.OAuth;
using MyJobLeads.DomainModel.LibSupport.DotNetOpenAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using System.Xml.Linq;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.Exceptions.OAuth;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Processes.ContactSearching
{
    public class LinkedInContactSearchProcesses : IProcess<LinkedInContactSearchParams, ExternalContactSearchResultsViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> _verifyOAuthProcess;

        public LinkedInContactSearchProcesses(MyJobLeadsDbContext context, IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> verifyOAuthProcess)
        {
            _context = context;
            _verifyOAuthProcess = verifyOAuthProcess;
        }

        /// <summary>
        /// Searches LinkedIn for contacts that match the specified criteria
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalContactSearchResultsViewModel Execute(LinkedInContactSearchParams procParams)
        {
            const int PAGE_SIZE = 10;

            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();

            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            if (!_verifyOAuthProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = user.Id }).AccessTokenValid)
                throw new UserHasNoValidOAuthAccessTokenException(user.Id, Enums.TokenProvider.LinkedIn);

            // Form the query URL
            string apiUrl = string.Format("http://api.linkedin.com/v1/people-search?keywords={0}&first-name={1}&last-name={2}&company-name={3}&title={4}&school-name={5}&country-code={6}&postal-code={7}&start={8}&count={9}",
                                HttpUtility.UrlEncode(procParams.Keywords),
                                HttpUtility.UrlEncode(procParams.FirstName),
                                HttpUtility.UrlEncode(procParams.LastName),
                                HttpUtility.UrlEncode(procParams.CompanyName),
                                HttpUtility.UrlEncode(procParams.Title),
                                HttpUtility.UrlEncode(procParams.SchoolName),
                                HttpUtility.UrlEncode(procParams.CountryCode),
                                HttpUtility.UrlEncode(procParams.PostalCode),
                                procParams.RequestedPageNumber * PAGE_SIZE,
                                PAGE_SIZE);

            if (procParams.RestrictToCurrentCompany)
                apiUrl += "&current-company=true";

            if (procParams.RestrictToCurrentSchool)
                apiUrl += "&current-school=true";

            if (procParams.RestrictToCurrentTitle)
                apiUrl += "&current-title=true";

            // Perform the search
            var consumer = new WebConsumer(LinkedInOAuthProcesses.GetLiDescription(), new LinkedInTokenManager(_context));
            var endpoint = new MessageReceivingEndpoint(apiUrl, HttpDeliveryMethods.GetRequest);
            var request = consumer.PrepareAuthorizedRequest(endpoint, user.LinkedInOAuthData.Token);
            var response = request.GetResponse();

            // Get the results from the respones
            var xmlResponse = XDocument.Load(response.GetResponseStream());
            var search = xmlResponse.Element("people-search");
            var resultsVm = new ExternalContactSearchResultsViewModel
            {
                PageSize = PAGE_SIZE,
                DisplayedPageNumber = procParams.RequestedPageNumber,
                TotalResultsCount = Convert.ToInt32(search.Element("people").Attribute("total").Value)
            };

            resultsVm.Results = search.Descendants("person")
                                      .Select(x => new ExternalContactSearchResultsViewModel.ContactResultViewModel
                                      {
                                          ContactId = x.Element("id").Value,
                                          FirstName = x.Element("first-name").Value,
                                          LastName = x.Element("last-name").Value
                                      })
                                      .ToList();

            return resultsVm;
        }
    }
}
