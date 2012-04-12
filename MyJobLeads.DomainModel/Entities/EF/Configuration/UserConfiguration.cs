using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace MyJobLeads.DomainModel.Entities.EF.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasMany(x => x.CreatedOrders).WithRequired(x => x.OrderedBy).HasForeignKey(x => x.OrderedById).WillCascadeOnDelete(false);
            HasMany(x => x.OwnedOrders).WithRequired(x => x.OrderedFor).HasForeignKey(x => x.OrderedForId).WillCascadeOnDelete(false);
            HasOptional(x => x.LinkedInOAuthData).WithMany(x => x.LinkedInUsers).HasForeignKey(x => x.LinkedInOAuthDataId);
            HasOptional(x => x.JigsawAccountDetails).WithRequired(x => x.AssociatedUser);
        }
    }
}
