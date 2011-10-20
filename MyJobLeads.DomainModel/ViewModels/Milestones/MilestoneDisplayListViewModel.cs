using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ViewModels.Milestones
{
    public class MilestoneDisplayListViewModel
    {
        public MilestoneDisplayListViewModel()
        {
            Milestones = new List<MilestoneSummaryViewModel>();
        }

        public IList<MilestoneSummaryViewModel> Milestones { get; set; }

        public class MilestoneSummaryViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
