using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Ordering
{
    public class OrderLicensesAlreadyActivatedException : MJLException
    {
        public OrderLicensesAlreadyActivatedException(int orderId)
            : base(string.Format("Order {0} has already has its licenses activated", orderId))
        {
            OrderId = orderId;
        }

        public int OrderId { get; set; }
    }
}
