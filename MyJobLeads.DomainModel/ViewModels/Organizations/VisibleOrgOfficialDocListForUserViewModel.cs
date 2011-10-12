using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.ViewModels.Organizations
{
    public class VisibleOrgOfficialDocListForUserViewModel
    {
        public IList<OfficialDocument> UserVisibleDocuments { get; set; }
    }
}
