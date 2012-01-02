using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class InvalidJigsawApiTokenException : MJLException
    {
        public InvalidJigsawApiTokenException(string apiToken)
            : base(string.Format("The Jigsaw API token '{0}' is invalid", apiToken))
        {
            ApiToken = apiToken;
        }

        public string ApiToken { get; set; }
    }
}
