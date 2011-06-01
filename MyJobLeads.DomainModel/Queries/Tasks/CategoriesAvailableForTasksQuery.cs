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
            return new List<string> { "Apply At Firm", "Follow Up", "In-Person Interview", "Initial Contact", "Phone Interview", "Other" };
        }
    }
}
