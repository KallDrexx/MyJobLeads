using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.DomainModel.Entities.Ordering
{
    public class Product
    {
        public int Id { get; set; }
        public ProductType Type { get; set; }
        public bool TimeRestricted { get; set; }
        public int DurationInWeeks { get; set; }
        public decimal Price { get; set; }
        public bool MaxPurchaseTimes { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<OrderedProduct> Orders { get; set; }
    }
}
