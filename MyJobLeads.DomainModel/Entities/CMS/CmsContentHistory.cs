using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.History;

namespace MyJobLeads.DomainModel.Entities.CMS
{
    public class CmsContentHistory : EntityHistoryBase
    {
        public string Title { get; set; }
        public string Html { get; set; }

        public virtual CmsContent CmsContent { get; set; }
        public int CmsContentId { get; set; }
    }
}
