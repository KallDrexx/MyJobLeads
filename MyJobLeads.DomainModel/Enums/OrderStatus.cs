using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Enums
{
    public enum OrderStatus
    {
        None = 0,
        AwaitingPayment = 1,
        PaymentFailed = 2,
        Completed = 3,
        Cancelled = 4,
        Refunded = 5
    }
}
