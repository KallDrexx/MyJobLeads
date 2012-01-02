using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw
{
    public class AddJigsawContactToJobSearchParams
    {
        public int RequestingUserId { get; set; }
        public bool CreateCompanyFromJigsaw { get; set; }
        public int ExistingCompanyId { get; set; }

        public string JigsawContactId { get; set; }
        public string JigsawCompanyId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
