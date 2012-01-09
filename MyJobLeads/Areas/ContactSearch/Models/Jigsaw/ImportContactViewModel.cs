using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.ContactSearch.Models.Jigsaw
{
    public class ImportContactViewModel
    {
        public bool CreateNewContact { get; set; }
        public int ExistingContactId { get; set; }
        public int JigsawContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string JigsawCompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime JigsawUpdatedDate { get; set; }

        public IList<SelectListItem> ExistingContactList { get; set; }

        public void SetExistingContactList(IList<Contact> contacts)
        {
            ExistingContactList = contacts.Select(x => new SelectListItem
                                            {
                                                Value = x.Id.ToString(),
                                                Text = string.Concat(x.Name, " (", x.Company.Name, ")")
                                            })
                                            .ToList();
            ExistingContactList.Insert(0, new SelectListItem { Value = "0", Text = "<None Selected>" });
        }
    }
}