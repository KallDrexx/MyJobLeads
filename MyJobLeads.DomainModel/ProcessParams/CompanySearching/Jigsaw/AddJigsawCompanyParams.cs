﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.CompanySearching.Jigsaw
{
    public class AddJigsawCompanyParams
    {
        public int RequestingUserId { get; set; }
        public int JigsawId { get; set; }
    }
}
