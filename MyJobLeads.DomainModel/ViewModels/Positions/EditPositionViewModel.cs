using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.ViewModels.Companies;

namespace MyJobLeads.ViewModels.Positions
{
    public class EditPositionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool HasApplied { get; set; }
        public string Notes { get; set; }
        public int RequestedUserId { get; set; }

        public CompanySummaryViewModel Company { get; set; }
    }
}