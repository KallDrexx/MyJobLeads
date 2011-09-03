using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.Positions
{
    public class PositionListViewModel
    {
        public IList<PositionDisplayViewModel> Positions { get; set; }
    }
}
