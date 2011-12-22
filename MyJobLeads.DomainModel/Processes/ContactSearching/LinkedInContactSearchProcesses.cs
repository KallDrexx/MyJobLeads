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
    public class LinkedInContactSearchProcesses : IProcess<LinkedInContactSearchParams, ExternalContactSearchResultsViewModel>,
                                                  IProcess<LinkedInContactDetailsParams, ExternalContactSearchResultsViewModel>
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
            string apiUrl = string.Format("http://api.linkedin.com/v1/people-search:{0}?keywords={1}&first-name={2}&last-name={3}&company-name={4}&title={5}&school-name={6}&country-code={7}&postal-code={8}&start={9}&count={10}&facet={11}",
                                "(people:(id,first-name,last-name,headline,api-standard-profile-request,site-standard-profile-request,public-profile-url))",
                                HttpUtility.UrlEncode(procParams.Keywords),
                                HttpUtility.UrlEncode(procParams.FirstName),
                                HttpUtility.UrlEncode(procParams.LastName),
                                HttpUtility.UrlEncode(procParams.CompanyName),
                                HttpUtility.UrlEncode(procParams.Title),
                                HttpUtility.UrlEncode(procParams.SchoolName),
                                HttpUtility.UrlEncode(procParams.CountryCode),
                                HttpUtility.UrlEncode(procParams.PostalCode),
                                procParams.RequestedPageNumber * PAGE_SIZE,
                                PAGE_SIZE,
                                HttpUtility.UrlEncode("&network=in"));

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
                                          LastName = x.Element("last-name").Value,
                                          Headline = x.Element("headline") != null ? x.Element("headline").Value : string.Empty,
                                          PublicUrl = x.Element("public-profile-url").Value
                                      })
                                      .ToList();

            return resultsVm;
        }

        /// <summary>
        /// Retrieves the details of a contact from LinkedIn
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalContactSearchResultsViewModel Execute(LinkedInContactDetailsParams procParams)
        {
            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();

            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            if (!_verifyOAuthProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = user.Id }).AccessTokenValid)
                throw new UserHasNoValidOAuthAccessTokenException(user.Id, Enums.TokenProvider.LinkedIn);

            // Form the query URL
            //string apiUrl = string.Format("http://api.linkedin.com/v1/people/url={0}", HttpUtility.UrlEncode(procParams.PublicUrl));
            string apiUrl = string.Format("http://api.linkedin.com/v1/people/url={0}:(first-name,last-name,headline,summary,{1},{2},{3})",
                                    HttpUtility.UrlEncode(procParams.PublicUrl),
                                    "educations:(school-name)",
                                    "site-standard-profile-request:(url)",
                                    "positions:(title,summary,is-current,start-date,end-date,company:(id,name))");

            // Retrieve the details
            var consumer = new WebConsumer(LinkedInOAuthProcesses.GetLiDescription(), new LinkedInTokenManager(_context));
            var endpoint = new MessageReceivingEndpoint(apiUrl, HttpDeliveryMethods.GetRequest);
            var request = consumer.PrepareAuthorizedRequest(endpoint, user.LinkedInOAuthData.Token);
            var response = request.GetResponse();

            // Get the results from the respones
            var xmlResponse = XDocument.Load(response.GetResponseStream());

            return null;
        }
    }
}
