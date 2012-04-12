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
        public int TypeValue { get; set; }
        public bool TimeRestricted { get; set; }
        public int DurationInWeeks { get; set; }
        public decimal Price { get; set; }
        public int MaxPurchaseTimes { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<OrderedProduct> Orders { get; set; }

        public ProductType Type
        {
            get { return (ProductType)TypeValue; }
            set { TypeValue = (int)value; }
        }
    }
}
