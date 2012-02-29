using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.CMS
{
    public class CmsContent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }

        public virtual IList<CmsContentHistory> History { get; set; }
    }
}
