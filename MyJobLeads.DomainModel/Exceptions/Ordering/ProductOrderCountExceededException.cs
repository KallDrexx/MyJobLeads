using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Ordering
{
    public class ProductOrderCountExceededException : MJLException
    {
        public ProductOrderCountExceededException(int userId, int productId)
            : base(string.Format("User {0} attempted to exceed the max activations for product {1}", userId, productId))
        {
            UserId = userId;
            ProductId = productId;
        }

        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
