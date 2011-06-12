using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.ViewModels
{
    public class JobSearchMilestoneProgress
    {
        public JobSearchMilestoneProgress(JobSearch jobSearch)
        {
            // Use the job search to figure out the progess of the current milestone
            var msMetrics = jobSearch.CurrentMilestone.JobSearchMetrics;
            var jsMetrics = jobSearch.Metrics;

            // Number of companies created
            if (msMetrics.NumCompaniesCreated == 0 || jsMetrics.NumCompaniesCreated > msMetrics.NumCompaniesCreated)
                NumCompaniesCreatedProgress = 1;
            else
                NumCompaniesCreatedProgress = (decimal)jsMetrics.NumCompaniesCreated / (decimal)msMetrics.NumCompaniesCreated;

            // Number of contacts created
            if (msMetrics.NumContactsCreated == 0 || jsMetrics.NumContactsCreated > msMetrics.NumContactsCreated)
                NumContactsCreatedProgress = 1;
            else
                NumContactsCreatedProgress = (decimal)jsMetrics.NumContactsCreated / (decimal)msMetrics.NumContactsCreated;

            // Number of application tasks created
            if (msMetrics.NumApplyTasksCreated == 0 || jsMetrics.NumApplyTasksCreated > msMetrics.NumApplyTasksCreated)
                NumApplyTasksCreatedProgress = 1;
            else
                NumApplyTasksCreatedProgress = (decimal)jsMetrics.NumApplyTasksCreated / (decimal)msMetrics.NumApplyTasksCreated;

            // Number of application tasks completed
            if (msMetrics.NumApplyTasksCompleted == 0 || jsMetrics.NumApplyTasksCompleted > msMetrics.NumApplyTasksCompleted)
                NumApplyTasksCompletedProgress = 1;
            else
                NumApplyTasksCompletedProgress = (decimal)jsMetrics.NumApplyTasksCompleted / (decimal)msMetrics.NumApplyTasksCompleted;

            // Number of phone interview tasks created
            if (msMetrics.NumPhoneInterviewTasksCreated == 0 || jsMetrics.NumPhoneInterviewTasksCreated > msMetrics.NumPhoneInterviewTasksCreated)
                NumPhoneInterviewTasksCreatedProgress = 1;
            else
                NumPhoneInterviewTasksCreatedProgress = (decimal)jsMetrics.NumPhoneInterviewTasksCreated / (decimal)msMetrics.NumPhoneInterviewTasksCreated; 

            // Number of in person interview tasks created
            if (msMetrics.NumInPersonInterviewTasksCreated == 0 || jsMetrics.NumInPersonInterviewTasksCreated > msMetrics.NumInPersonInterviewTasksCreated)
                NumInPersonInterviewTasksCreatedProgress = 1;
            else
                NumInPersonInterviewTasksCreatedProgress = (decimal)jsMetrics.NumInPersonInterviewTasksCreated / (decimal)msMetrics.NumInPersonInterviewTasksCreated; 

            // Deterine the final total progress for the milestone, only counting milestones whose metrics are greater than zero
            decimal totalprogress = 0;
            int countMetricsUsed = 0;

            if (msMetrics.NumApplyTasksCompleted > 0)
            {
                totalprogress += NumApplyTasksCompletedProgress;
                countMetricsUsed++;
            }

            if (msMetrics.NumApplyTasksCreated > 0)
            {
                totalprogress += NumApplyTasksCreatedProgress;
                countMetricsUsed++;
            }

            if (msMetrics.NumCompaniesCreated > 0)
            {
                totalprogress += NumCompaniesCreatedProgress;
                countMetricsUsed++;
            }

            if (msMetrics.NumContactsCreated > 0)
            {
                totalprogress += NumContactsCreatedProgress;
                countMetricsUsed++;
            }

            if (msMetrics.NumInPersonInterviewTasksCreated > 0)
            {
                totalprogress += NumInPersonInterviewTasksCreatedProgress;
                countMetricsUsed++;
            }

            if (msMetrics.NumPhoneInterviewTasksCreated > 0)
            {
                totalprogress += NumPhoneInterviewTasksCreatedProgress;
                countMetricsUsed++;
            }

            // Calculate the average of all the used progresses
            if (countMetricsUsed == 0)
                TotalProgress = 1;
            else
                TotalProgress = totalprogress / countMetricsUsed;
        }

        public decimal TotalProgress { get; protected set; }

        public decimal NumCompaniesCreatedProgress { get; protected set; }
        public decimal NumContactsCreatedProgress { get; protected set; }
        public decimal NumApplyTasksCreatedProgress { get; protected set; }
        public decimal NumApplyTasksCompletedProgress { get; protected set; }
        public decimal NumPhoneInterviewTasksCreatedProgress { get; protected set; }
        public decimal NumInPersonInterviewTasksCreatedProgress { get; protected set; }
    }
}
