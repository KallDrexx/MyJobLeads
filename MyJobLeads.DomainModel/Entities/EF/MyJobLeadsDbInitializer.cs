using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Database;

namespace MyJobLeads.DomainModel.Entities.EF
{
    public class MyJobLeadsDbInitializer : DropCreateDatabaseIfModelChanges<MyJobLeadsDbContext>
    {
        /// <summary>
        /// Seeds the data context with default data when database is created
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(MyJobLeadsDbContext context)
        {
        }
    }
}
