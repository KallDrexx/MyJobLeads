using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Entities.EF;
using DotNetOpenAuth.OAuth.Messages;
using MyJobLeads.DomainModel.Entities;
using System.Configuration;

namespace MyJobLeads.DomainModel.LibSupport.DotNetOpenAuth
{
    public abstract class MyJobLeadsBaseConsumerTokenManager : IConsumerTokenManager
    {
        protected string _consumerKeyAppSettingName;
        protected string _consumerSecretAppSettingName;
        protected MyJobLeadsDbContext _context;

        public string ConsumerKey
        {
            get { return ConfigurationManager.AppSettings[_consumerKeyAppSettingName]; }
        }

        public string ConsumerSecret
        {
            get { return ConfigurationManager.AppSettings[_consumerSecretAppSettingName]; }
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
            var oldData = _context.OAuthData.Where(x => x.Token == requestToken && x.TokenTypeValue == (int)TokenType.RequestToken).Single();
            _context.OAuthData.Remove(oldData);

            // Create the access token
            _context.OAuthData.Add(new OAuthData
            {
                Token = accessToken,
                Secret = accessTokenSecret,
                TokenType = TokenType.AccessToken
            });

            _context.SaveChanges();
        }

        public string GetTokenSecret(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("token is null or empty");

            var data = _context.OAuthData.Where(x => x.Token == token).SingleOrDefault();
            if (data == null)
                throw new ArgumentException("No secret found for the given token");

            return data.Secret;
        }

        public TokenType GetTokenType(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("token is null or empty");

            var data = _context.OAuthData.Where(x => x.Token == token).SingleOrDefault();
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

            var data = new OAuthData
            {
                Token = response.Token,
                Secret = response.TokenSecret,
                TokenType = TokenType.RequestToken
            };

            _context.OAuthData.Add(data);
            _context.SaveChanges();
        }
    }
}
