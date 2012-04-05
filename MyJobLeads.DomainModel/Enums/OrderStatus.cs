using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Enums
{
    public enum OrderStatus
    {
        None = 0,
        NotConfirmed = 1,
        Processing = 2,
        Paid = 3,
        Refunded = 4,
        Cancelled = 5
    }
}
