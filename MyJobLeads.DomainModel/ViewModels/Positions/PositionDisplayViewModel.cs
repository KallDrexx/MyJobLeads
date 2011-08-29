using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.ViewModels.Companies;

namespace MyJobLeads.DomainModel.ViewModels.Positions
{
    public class PositionDisplayViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool HasApplied { get; set; }
        public string Notes { get; set; }

        public CompanySummaryViewModel Company { get; set; }
    }
}
