﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn
{
    public class LinkedInPositionDetailsParams
    {
        public int RequestingUserId { get; set; }
        public string PositionId { get; set; }
    }
}
