using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Queries.Tasks
{
    /// <summary>
    /// Query that retrieves categories that are available for tasks
    /// </summary>
    public class CategoriesAvailableForTasksQuery
    {
        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public IList<string> Execute()
        {
            // Current category list is hardcoded
            return new List<string> 
            { 
                MJLConstants.ApplyToFirmTaskCategory,
                MJLConstants.FollowUpTaskCategory,
                MJLConstants.InPersonInterviewTaskCategory,
                MJLConstants.InitialContactTaskCategory,
                MJLConstants.PhoneInterviewTaskCategory,
                MJLConstants.OtherTaskCategory
            };
        }
    }
}
