using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.DomainModel.Entities
{
    public class OAuthData
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Secret { get; set; }
        public int TokenTypeValue { get; set; }
        public int TokenProviderValue { get; set; }

        public virtual User LinkedInUser { get; set; }

        public TokenType TokenType
        {
            get { return (TokenType)TokenTypeValue; }
            set { TokenTypeValue = (int)value; }
        }

        public TokenProvider TokenProvider
        {
            get { return (TokenProvider)TokenProviderValue; }
            set { TokenProviderValue = (int)value; }
        }
    }
}
