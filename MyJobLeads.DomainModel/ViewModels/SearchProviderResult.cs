using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels
{
    public class SearchProviderResult
    {
        public SearchProviderResult()
        {
            FoundCompanyIds = new List<int>();
            FoundContactIds = new List<int>();
            FoundTaskIds = new List<int>();
        }

        public IList<int> FoundCompanyIds { get; protected set; }
        public IList<int> FoundContactIds { get; protected set; }
        public IList<int> FoundTaskIds { get; protected set; }
    }
}
