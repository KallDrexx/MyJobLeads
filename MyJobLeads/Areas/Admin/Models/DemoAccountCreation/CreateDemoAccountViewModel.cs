using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.Areas.Admin.Models.DemoAccountCreation
{
    public class CreateDemoAccountViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Both passwords must match")]
        public string ConfirmPassword { get; set; }
    }
}