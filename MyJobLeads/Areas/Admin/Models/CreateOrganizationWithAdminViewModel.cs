using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyJobLeads.Areas.Admin.Models
{
    public class CreateOrganizationWithAdminViewModel
    {
        [Required]
        public string OrganizationName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string AdminEmail { get; set; }

        [Required]
        public string AdminName { get; set; }

        [Required]
        public string AdminPlainTextPassword { get; set; }
    }
}