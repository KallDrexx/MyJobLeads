using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class JigsawCompanyNotFoundException : JigsawException
    {
        public JigsawCompanyNotFoundException(string companyId)
            : base(string.Format("No jigsaw company was found with an id of {0}", companyId))
        {
            CompanyId = companyId;
        }

        public string CompanyId { get; set; }
    }
}
