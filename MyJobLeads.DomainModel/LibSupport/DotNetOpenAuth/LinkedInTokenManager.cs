using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.DomainModel.LibSupport.DotNetOpenAuth
{
    public class LinkedInTokenManager : MyJobLeadsBaseConsumerTokenManager
    {
        private const string LINKED_IN_KEY_APPSETTINGS = "LiApiKey";
        private const string LINKED_IN_SECRET_APPSETTINGS = "LiApiSecret";

        public LinkedInTokenManager(MyJobLeadsDbContext context, int requestedingUserId)
            : base(context, TokenProvider.LinkedIn, LINKED_IN_KEY_APPSETTINGS, LINKED_IN_SECRET_APPSETTINGS, requestedingUserId)
        {
        }
    }
}
