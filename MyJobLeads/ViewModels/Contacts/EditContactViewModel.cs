using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.ViewModels.Contacts
{
    public class EditContactViewModel
    {
        public Contact Contact { get; set; }
        public Company Company { get; set; }
    }
}
