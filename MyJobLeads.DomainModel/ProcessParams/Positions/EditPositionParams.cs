using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.Positions
{
    public struct EditPositionParams
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool HasApplied { get; set; }
        public string Notes { get; set; }
        public int RequestingUserId { get; set; }
    }
}
