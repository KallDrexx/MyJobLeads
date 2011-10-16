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
            Milestones = new List<MilestoneDisplayViewModel>();
        }

        public IList<MilestoneDisplayViewModel> Milestones { get; set; }

        public class MilestoneDisplayViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
