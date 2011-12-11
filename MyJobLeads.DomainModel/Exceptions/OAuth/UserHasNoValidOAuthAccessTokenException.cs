using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.DomainModel.Exceptions.OAuth
{
    public class UserHasNoValidOAuthAccessTokenException : MJLException
    {
        public UserHasNoValidOAuthAccessTokenException(int userId, TokenProvider tokenProvider) 
            : base(string.Format("User Id {0} does not have a valid OAuth access token for the {1} provider", userId, tokenProvider.ToString()))
        {
            UserId = userId;
            TokenProvider = tokenProvider;
        }

        public int UserId { get; protected set; }
        public TokenProvider TokenProvider { get; protected set; }
    }
}
