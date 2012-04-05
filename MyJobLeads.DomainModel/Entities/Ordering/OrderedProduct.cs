using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.Ordering
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        public decimal Price { get; set; }

        public virtual Order Order { get; set; }
        public int OrderId { get; set; }

        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
