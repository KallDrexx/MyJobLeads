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

namespace MyJobLeads.DomainModel.Processes.PositionSearching
{
    public class LinkedInPositionSearchProcesses : IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public LinkedInPositionSearchProcesses(MyJobLeadsDbContext context)
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
            return new UserAccessTokenResultViewModel
            {
                AccessTokenValid = UserHasValidAccessToken(procParams.UserId)
            };
        }

        protected bool UserHasValidAccessToken(int userId)
        {
            var user = _context.Users
                               .Where(x => x.Id == userId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();
            if (user == null)
                return false;

            if (user.LinkedInOAuthData == null)
                return false;

            // Attempt to query for the current user's profile to determine if the profile is valid
            var consumer = new WebConsumer(GetLiDescription(), new LinkedInTokenManager(_context));
            var endpoint = new MessageReceivingEndpoint("http://api.linkedin.com/v1/people/~", HttpDeliveryMethods.GetRequest);
            var request = consumer.PrepareAuthorizedRequest(endpoint, user.LinkedInOAuthData.Token);
            try { var response = request.GetResponse(); }
            catch (WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    return false;

                throw;
            }

            return true;
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
