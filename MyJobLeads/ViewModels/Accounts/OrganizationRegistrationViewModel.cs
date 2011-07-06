using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyJobLeads.ViewModels.Accounts
{
    public class OrganizationRegistrationViewModel
    {
        public OrganizationRegistrationViewModel()
        {
            RestrictedEmailDomains = new List<string>();
        }

        public int MinPasswordLength { get; set; }
        public Guid RegistrationToken { get; set; }
        public string OrganizationName { get; set; }

        public IList<string> RestrictedEmailDomains { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}