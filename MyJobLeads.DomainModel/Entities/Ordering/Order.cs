﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Enums;
using MyJobLeads.DomainModel.Entities.FillPerfect;

namespace MyJobLeads.DomainModel.Entities.Ordering
{
    public class Order
    {
        public Order()
        {
            OrderedProducts = new List<OrderedProduct>();
            FillPerfectLicenses = new List<FpUserLicense>();
        }

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int OrderStatusValue { get; set; }
        public string PayPalToken { get; set; }
        public string PayPalTransactionId { get; set; }

        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }
        public virtual ICollection<FpUserLicense> FillPerfectLicenses { get; set; }

        public virtual User OrderedBy { get; set; }
        public int OrderedById { get; set; }

        public virtual User OrderedFor { get; set; }
        public int OrderedForId { get; set; }

        public OrderStatus OrderStatus
        {
            get { return (OrderStatus)OrderStatusValue; }
            set { OrderStatusValue = (int)value; }
        }
    }
}