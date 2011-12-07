using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.LibSupport.DotNetOpenAuth;
using System.Net;
using System.Web;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Enums;
using System.Xml;
using System.Xml.Linq;
using MyJobLeads.DomainModel.Exceptions.OAuth;

namespace MyJobLeads.DomainModel.Processes.PositionSearching
{
    public class LinkedInPositionSearchProcesses 
        : IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>,
          IProcess<StartLinkedInUserAuthParams, GeneralSuccessResultViewModel>,
          IProcess<ProcessLinkedInAuthProcessParams, GeneralSuccessResultViewModel>,
          IProcess<LinkedInPositionSearchParams, PositionSearchResultsViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> _verifyLiTokenProcess;

        public LinkedInPositionSearchProcesses(MyJobLeadsDbContext context, 
                            IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> verifyLiTokenProcess )
        {
            _context = context;
            _verifyLiTokenProcess = verifyLiTokenProcess;
        }

        /// <summary>
        /// Verifies the specified user has a valid linked in access token
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public UserAccessTokenResultViewModel Execute(VerifyUserLinkedInAccessTokenParams procParams)
        {
            var user = _context.Users
                               .Where(x => x.Id == procParams.UserId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();
            if (user == null)
                return new UserAccessTokenResultViewModel { AccessTokenValid = false };

            if (user.LinkedInOAuthData == null)
                return new UserAccessTokenResultViewModel { AccessTokenValid = false };

            // Attempt to query for the current user's profile to determine if the profile is valid
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context));
            var endpoint = new MessageReceivingEndpoint("http://api.linkedin.com/v1/people/~", HttpDeliveryMethods.GetRequest);
            var request = consumer.PrepareAuthorizedRequest(endpoint, user.LinkedInOAuthData.Token);
            try { var response = request.GetResponse(); }
            catch (WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    return new UserAccessTokenResultViewModel { AccessTokenValid = false };

                throw;
            }

            return new UserAccessTokenResultViewModel { AccessTokenValid = true };
        }

        /// <summary>
        /// Begins the Linked In OAuth process
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(StartLinkedInUserAuthParams procParams)
        {
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context));
            consumer.Channel.Send(consumer.PrepareRequestUserAuthorization(procParams.ReturnUrl, null, null));
            return new GeneralSuccessResultViewModel();
        }

        /// <summary>
        /// Processes the LinkedIn Authorization process
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(ProcessLinkedInAuthProcessParams procParams)
        {
            // Get the user for authentication
            var user = _context.Users.Where(x => x.Id == procParams.UserId).Include(x => x.LinkedInOAuthData).SingleOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.UserId);

            // Process the completed Linked In Auth
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context));
            var accessTokenResponse = consumer.ProcessUserAuthorization();

            if (accessTokenResponse == null)
                throw new OAuthResponseNotAvailableException("Linked In access token response was not provided");

            // Get the access token and associate the OAuthData record with the user
            string accessToken = accessTokenResponse.AccessToken;
            var oAuthRecord = _context.OAuthData
                                      .Where(x => x.Token == accessToken && x.TokenTypeValue == (int)TokenType.AccessToken && x.TokenProviderValue == (int)TokenProvider.LinkedIn)
                                      .SingleOrDefault();
            if (oAuthRecord == null)
                throw new MJLEntityNotFoundException(typeof(OAuthData), accessToken);

            // If the user is already associated with an OAuthData record, delete that recor
            if (user.LinkedInOAuthData != null)
                _context.OAuthData.Remove(user.LinkedInOAuthData);

            user.LinkedInOAuthData = oAuthRecord;
            _context.SaveChanges();

            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }

        /// <summary>
        /// Executes the linked in position search for the user 
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public PositionSearchResultsViewModel Execute(LinkedInPositionSearchParams procParams)
        {
            const int resultsPageSize = 10;

            // Get the user's access token
            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();

            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            if (!_verifyLiTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = user.Id }).AccessTokenValid)
                throw new UserHasNoValidOAuthAccessTokenException(user.Id, TokenProvider.LinkedIn);

            // Form the API Url based on the criteria
            string apiUrl = string.Format("http://api.linkedin.com/v1/job-search?country-code={0}&Keywords={1}&start={2}&count={3}", 
                                HttpUtility.UrlEncode(procParams.CountryCode), 
                                HttpUtility.UrlEncode(procParams.Keywords), 
                                procParams.ResultsPageNum,
                                resultsPageSize);

            if (!string.IsNullOrWhiteSpace(procParams.ZipCode))
                apiUrl += "&postal-code=" + procParams.ZipCode;

            // Perform the search
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context));
            var endpoint = new MessageReceivingEndpoint(apiUrl, HttpDeliveryMethods.GetRequest);
            var request = consumer.PrepareAuthorizedRequest(endpoint, user.LinkedInOAuthData.Token);
            var response = request.GetResponse();

            // Get the results from the respones
            var resultsVm = new PositionSearchResultsViewModel { ResultsPageNum = procParams.ResultsPageNum, DataSource = ExternalDataSource.LinkedIn };
            var xmlResponse = XDocument.Load(response.GetResponseStream());

            resultsVm.Results = (from job in xmlResponse.Descendants("job")
                                 select new PositionSearchResultsViewModel.PositionSearchResultViewModel
                                 {
                                     JobId = Convert.ToInt32(job.Element("id").Value),
                                     Headline = job.Element("job-poster").Element("headline").Value,
                                     Company = job.Element("company").Element("name").Value,
                                     Location = job.Element("location-description").Value,
                                     Description = job.Element("description-snippet").Value
                                 }).ToList();

            var searchStats = xmlResponse.Descendants("jobs").First();
            resultsVm.TotalCount = Convert.ToInt32(searchStats.Attribute("total").Value);
            resultsVm.ResultsPageNum = Convert.ToInt32(searchStats.Attribute("start").Value);
            
            //resultsVm.Results = xmlResponse.wh
            return resultsVm;
        }

        protected ServiceProviderDescription GetLiDescription()
        {
            return new ServiceProviderDescription
            {
                AccessTokenEndpoint = new MessageReceivingEndpoint("https://api.linkedin.com/uas/oauth/accessToken", HttpDeliveryMethods.PostRequest),
                RequestTokenEndpoint = new MessageReceivingEndpoint("https://api.linkedin.com/uas/oauth/requestToken", HttpDeliveryMethods.PostRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint("https://api.linkedin.com/uas/oauth/authorize", HttpDeliveryMethods.PostRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                ProtocolVersion = ProtocolVersion.V10a
            };
        }
    }
}
