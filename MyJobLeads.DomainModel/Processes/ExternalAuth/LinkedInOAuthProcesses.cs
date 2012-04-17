using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.LibSupport.DotNetOpenAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth;
using MyJobLeads.DomainModel.Enums;
using System.Net;

namespace MyJobLeads.DomainModel.Processes.ExternalAuth
{
    public class LinkedInOAuthProcesses : IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>,
          IProcess<StartLinkedInUserAuthParams, GeneralSuccessResultViewModel>,
          IProcess<ProcessLinkedInAuthProcessParams, GeneralSuccessResultViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public LinkedInOAuthProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
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
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context, procParams.UserId));
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
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context, procParams.RequestingUserId));
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
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context, procParams.UserId));
            var accessTokenResponse = consumer.ProcessUserAuthorization();

            if (accessTokenResponse == null)
                throw new OAuthResponseNotAvailableException("Linked In access token response was not provided");

            // Get the access token and associate the OAuthData record with the user
            string accessToken = accessTokenResponse.AccessToken;
            var oAuthRecord = _context.OAuthData
                                      .Where(x => x.Token == accessToken && x.TokenTypeValue == (int)TokenType.AccessToken)
                                      .Where(x => x.TokenProviderValue == (int)TokenProvider.LinkedIn)
                                      .SingleOrDefault();
            if (oAuthRecord == null)
                throw new MJLEntityNotFoundException(typeof(OAuthData), accessToken);

            // If the user is already associated with an OAuthData record, delete that recor
            if (user.LinkedInOAuthData != null)
                if (user.LinkedInOAuthData.Token != accessToken)
                    _context.OAuthData.Remove(user.LinkedInOAuthData);

            user.LinkedInOAuthData = oAuthRecord;
            _context.SaveChanges();

            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }

        public static ServiceProviderDescription GetLiDescription()
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
