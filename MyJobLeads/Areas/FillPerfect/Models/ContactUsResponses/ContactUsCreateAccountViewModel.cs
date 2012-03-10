using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.Areas.Admin.Models.DemoAccountCreation;
using MyJobLeads.ViewModels.Admin;

namespace MyJobLeads.Areas.FillPerfect.Models.ContactUsResponses
{
    public class ContactUsCreateAccountViewModel
    {
        public ContactUsCreateAccountViewModel()
        {
            CreateAccountModel = new CreateDemoAccountViewModel();
        }

        public int FpContactResponseId { get; set; }
        public string ResponseName { get; set; }
        public string ResponseEmail { get; set; }
        public string ResponseSchool { get; set; }
        public string ResponseProgram { get; set; }
        public CreateDemoAccountViewModel CreateAccountModel { get; set; }
    }
}