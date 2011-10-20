using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities.Metrics;
using System.Web.Mvc;
using MyJobLeads.DomainModel.ViewModels.Milestones;

namespace MyJobLeads.ViewModels.Milestones
{
    public class EditMilestoneViewModel
    {
        public EditMilestoneViewModel()
        {
            JobSearchMetrics = new JobSearchMetrics();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public string CompletionDisplay { get; set; }
        public int PreviousMilestoneId { get; set; }
        public JobSearchMetrics JobSearchMetrics { get; set; }
        public IList<SelectListItem> AvailableMilestoneList { get; set; }

        public void SetAvailableMilestoneList(MilestoneDisplayListViewModel milestoneList)
        {
            AvailableMilestoneList = milestoneList.Milestones
                                                  .Where(x => x.Id != Id)
                                                  .Select(x => new SelectListItem
                                                  {
                                                      Value = x.Id.ToString(),
                                                      Text = x.Name,
                                                      Selected = x.Id == PreviousMilestoneId
                                                  })
                                                  .ToList();

            AvailableMilestoneList.Insert(0, new SelectListItem { Value = "0", Text = "<No Previous Milestone>" });
        }
    }
}