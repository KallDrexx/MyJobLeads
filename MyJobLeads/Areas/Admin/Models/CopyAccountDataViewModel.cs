using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.Admin.Models
{
    public class CopyAccountDataViewModel
    {
        public int SelectedAccountId { get; set; }
        public IList<SelectListItem> AccountList { get; set; }
        public string NewUsername { get; set; }
        public string Password { get; set; }
        public bool CreateAsOrgAdmin { get; set; }

        public void SetAccountList(IList<User> users)
        {
            AccountList = users.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Email })
                               .OrderBy(x => x.Text)
                               .ToList();
        }
    }
}