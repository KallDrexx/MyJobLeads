using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Entities.EF;
using DotNetOpenAuth.OAuth.Messages;
using MyJobLeads.DomainModel.Entities;
using System.Configuration;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.DomainModel.LibSupport.DotNetOpenAuth
{
    public abstract class MyJobLeadsBaseConsumerTokenManager : IConsumerTokenManager
    {
        private string _consumerKeyAppSettingName;
        private string _consumerSecretAppSettingName;
        private MyJobLeadsDbContext _context;
        private TokenProvider _tokenProvider;
        private int _requestingUserId;

        public MyJobLeadsBaseConsumerTokenManager(MyJobLeadsDbContext context, TokenProvider tokenProvider, 
                                                string consumerKeyAppSetting, string consumerSecretAppSetting, int requestingUserId)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (string.IsNullOrWhiteSpace(consumerKeyAppSetting))
                throw new ArgumentNullException("consumerKeyAppSetting");

            if (string.IsNullOrWhiteSpace(consumerSecretAppSetting))
                throw new ArgumentNullException("consumerSecretAppSetting");

            _context = context;
            _tokenProvider = tokenProvider;
            _consumerKeyAppSettingName = consumerKeyAppSetting;
            _consumerSecretAppSettingName = consumerSecretAppSetting;
            _requestingUserId = requestingUserId;
        }

        public string ConsumerKey
        {
            get { return ConfigurationManager.AppSettings[_consumerKeyAppSettingName]; }
        }

        public string ConsumerSecret
        {
            get { return ConfigurationManager.AppSettings[_consumerSecretAppSettingName]; }
        }

        public void DeleteAccessToken(string accessToken)
        {
            var data = _context.OAuthData
                                .Where(x => x.Token == accessToken && x.TokenProviderValue == (int)_tokenProvider)
                                .Include(x => x.LinkedInUsers)
                                .SingleOrDefault();

            if (data == null)
                return;

            // Make sure the user is no longer associated with the token
            if (_tokenProvider == TokenProvider.LinkedIn)
                data.LinkedInUsers.Clear();

            _context.OAuthData.Remove(data);
            _context.SaveChanges();
        }

        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
        {
            if (string.IsNullOrEmpty(consumerKey))
                throw new ArgumentException("consumerKey was null or empty");

            if (string.IsNullOrEmpty(requestToken))
                throw new ArgumentException("requestToken was null or empty");

            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentException("accessToken was null or empty");

            if (accessTokenSecret == null)
                throw new ArgumentNullException("accessTokenSecret");

            // Remove the request token
            var oldData = _context.OAuthData.Where(x => x.Token == requestToken)
                                  .Where(x => x.TokenTypeValue == (int)TokenType.RequestToken)
                                  .Where(x => x.TokenProviderValue == (int)_tokenProvider)
                                  .Where(x => x.LinkedInUsers.Any(y => y.Id == _requestingUserId))
                                  .Single();

            _context.OAuthData.Remove(oldData);
            _context.SaveChanges();

            // Create the access token if one doesn't already exist 
            if (!_context.OAuthData.Any(x => x.Token == accessToken && x.TokenProviderValue == (int)_tokenProvider))
            {
                var user = _context.Users.Where(x => x.Id == _requestingUserId).First();
                _context.OAuthData.Add(new OAuthData
                {
                    Token = accessToken,
                    Secret = accessTokenSecret,
                    TokenType = TokenType.AccessToken,
                    TokenProvider = _tokenProvider,
                    LinkedInUsers = new List<User> { user }
                });

                _context.SaveChanges();
            }
        }

        public string GetTokenSecret(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("token is null or empty");

            var data = _context.OAuthData.Where(x => x.Token == token && x.TokenProviderValue == (int)_tokenProvider).SingleOrDefault();
            if (data == null)
                throw new ArgumentException("No secret found for the given token");

            return data.Secret;
        }

        public TokenType GetTokenType(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("token is null or empty");

            var data = _context.OAuthData.Where(x => x.Token == token)
                                         .Where(x => x.TokenProviderValue == (int)_tokenProvider)
                                         .Where(x => x.LinkedInUsers.Any(y => y.Id == _requestingUserId))
                                         .SingleOrDefault();
            if (data == null)
                throw new ArgumentException("No secret found for the given token");

            return data.TokenType;
        }

        public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (response == null)
                throw new ArgumentNullException("response");

            var user = _context.Users.Where(x => x.Id == _requestingUserId).First();
            var data = new OAuthData
            {
                Token = response.Token,
                Secret = response.TokenSecret,
                TokenType = TokenType.RequestToken,
                TokenProvider = _tokenProvider,
                LinkedInUsers = new List<User> { user }
            };

            _context.OAuthData.Add(data);
            _context.SaveChanges();
        }
    }
}
