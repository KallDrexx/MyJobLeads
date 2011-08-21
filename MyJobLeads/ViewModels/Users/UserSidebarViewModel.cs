using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.Controllers;

namespace MyJobLeads.ViewModels.Users
{
    public class UserSidebarViewModel
    {
        public UserSidebarViewModel() { }

        public UserSidebarViewModel(User user)
        {
            Id = user.Id;
            DisplayName = user.FullName;

            if (user.IsOrganizationAdmin)
                UserType = UserSidebarViewModel.AccountType.OrganizationAdmin;
            else
                UserType = UserSidebarViewModel.AccountType.User;
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public AccountType UserType { get; set; }
        public ActiveSidebarLink ActiveLink { get; set; }

        public enum AccountType { User, OrganizationAdmin, SiteAdmin }
    }
}