using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.Admin
{
    public class FillPerfectContactResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string School { get; set; }
        public string Program { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
