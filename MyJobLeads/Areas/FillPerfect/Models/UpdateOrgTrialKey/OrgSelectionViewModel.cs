using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyJobLeads.Areas.FillPerfect.Models.UpdateOrgTrialKey
{
    public class OrgSelectionViewModel
    {
        public int SelectedOrgId { get; set; }
        public IList<SelectListItem> OrganizationList { get; set; }

        public OrgSelectionViewModel() { }

        public void FormOrgList(IList<MyJobLeads.DomainModel.Entities.Organization> Organizations)
        {
            OrganizationList = Organizations
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}