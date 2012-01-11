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
        public bool CreateNewCompany { get; set; }

        public int SelectedContactId { get; set; }
        public int SelectedCompanyId { get; set; }

        public int JigsawContactId { get; set; }
        public string JigsawCompanyId { get; set; }
        public DateTime JigsawUpdatedDate { get; set; }

        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string CompanyName { get; set; }

        public IList<SelectListItem> ExistingContactList { get; set; }
        public IList<SelectListItem> ExistingCompanyList { get; set; }

        public void SetDropdownListValues(IEnumerable<Contact> contacts, IEnumerable<Company> companies)
        {
            ExistingContactList = contacts.Select(x => new SelectListItem
                                            {
                                                Value = x.Id.ToString(),
                                                Text = string.Concat(x.Name, " (", x.Company.Name, ")")
                                            })
                                            .ToList();
            ExistingContactList.Insert(0, new SelectListItem { Value = "0", Text = "<None Selected>" });

            ExistingCompanyList = companies.OrderBy(x => x.Name)
                                           .Select(x => new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString()
                                           })
                                            .ToList();
            ExistingCompanyList.Insert(0, new SelectListItem { Value = "0", Text = "<None Selected>" });
        }
    }
}