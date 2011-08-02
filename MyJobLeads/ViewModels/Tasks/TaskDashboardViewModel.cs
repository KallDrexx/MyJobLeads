using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.ViewModels.Tasks
{
    public class TaskDashboardViewModel
    {
        public TaskDashboardViewModel() { }

        public TaskDashboardViewModel(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var allTasks = user.LastVisitedJobSearch.Companies.SelectMany(x => x.Tasks).ToList();
            OverdueTasks = allTasks.Where(x => (x.TaskDate.HasValue && x.TaskDate.Value.Date < DateTime.Today) || !x.TaskDate.HasValue)
                                   .Where(x => x.CompletionDate == null)
                                   .ToList();

            TodaysTasks = allTasks.Where(x => x.TaskDate.HasValue && x.TaskDate.Value.Date == DateTime.Today)
                                  .Where(x => x.CompletionDate == null)
                                  .ToList();

            FutureTasks = allTasks.Where(x => x.TaskDate.HasValue && x.TaskDate.Value.Date > DateTime.Today)
                                  .Where(x => x.CompletionDate == null)
                                  .ToList();

            CurrentMilestone = user.LastVisitedJobSearch.CurrentMilestone;
        }

        public IList<Task> OverdueTasks;
        public IList<Task> TodaysTasks;
        public IList<Task> FutureTasks;
        public MilestoneConfig CurrentMilestone;
    }
}