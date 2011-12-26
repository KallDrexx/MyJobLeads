using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw
{
    public class JigsawContactSearchParams
    {
        public int RequestingUserId { get; set; }
        public int RequestedPageNum { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Levels { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string Departments { get; set; }
        public string Title { get; set; }
    }
}
