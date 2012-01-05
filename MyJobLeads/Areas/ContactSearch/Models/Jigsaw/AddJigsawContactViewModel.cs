using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.Areas.ContactSearch.Models
{
    public class AddJigsawContactViewModel
    {
        public bool CreateNewCompany { get; set; }
        public int SelectedCompanyId { get; set; }
        public IList<SelectListItem> ExistingCompanyList { get; set; }

        public string JigsawContactId { get; set; }
        public string JigsawCompanyId { get; set; }
        public string JigsawCompanyName { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public void SetExistingCompanyList(IList<Company> companies)
        {
            ExistingCompanyList = companies.OrderBy(x => x.Name)
                                           .Select(x => new SelectListItem
                                            {
                                                Text = x.Name,
                                                Value = x.Id.ToString()
                                            })
                                            .ToList();
        }
    }
}