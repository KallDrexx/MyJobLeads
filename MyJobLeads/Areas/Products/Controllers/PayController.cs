using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.Infrastructure.Attributes;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.ViewModels.Ordering;
using MyJobLeads.Areas.Products.Models;
using RestSharp;

namespace MyJobLeads.Areas.Products.Controllers
{
    [MJLAuthorize]
    public partial class PayController : MyJobLeadsBaseController
    {
        public PayController(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        public virtual ActionResult Index(int orderId)
        {
            var order = _context.Orders
                                .Where(x => x.Id == orderId && x.OrderedForId == CurrentUserId)
                                .FirstOrDefault();

            if (order == null || order.OrderStatus != DomainModel.Enums.OrderStatus.AwaitingPayment)
                return RedirectToAction(MVC.Home.Index());

            var model = new OrderUtils(_context, CurrentUserId).GetConfirmModel(order.OrderedProducts.First().ProductId);
            model.OrderId = orderId;
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(OrderConfirmViewModel model)
        {
            var order = _context.Orders
                                .Where(x => x.Id == model.OrderId && x.OrderedForId == CurrentUserId)
                                .FirstOrDefault();

            if (order == null || order.OrderStatus != DomainModel.Enums.OrderStatus.AwaitingPayment)
                return RedirectToAction(MVC.Home.Index());

            // Generate a paypal token
            order.PayPalToken = Guid.NewGuid();
            _context.SaveChanges();

            

            //var client = new RestClient("https://api-3t.sandbox.paypal.com/nvp");
            //var request = new RestRequest();
            //request.Parameters.Add(new Parameter { Name = "METHOD", Value = "DoExpressCheckoutPayment", Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "VERSION", Value = "92.0", Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "USER", Value = "seller_1341169691_biz_api1.interviewtools.net", Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "PWD", Value = "1341169715", Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "SIGNATURE", Value = "An5ns1Kso7MWUdW4ErQKJJJ4qi4-AMtzjW-w.HYrGhqS0OHTpv3HH3L2", Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "TOKEN", Value = order.PayPalToken.ToString(), Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "PAYMENTREQUEST_0_AMT", Value = order.TotalPrice.ToString(), Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "PAYMENTREQUEST_0_PAYMENTACTION", Value = "Sale", Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "RETURNURL", Value = Url.Action(MVC.Products.Pay.Index()), Type = ParameterType.GetOrPost });
            //request.Parameters.Add(new Parameter { Name = "CANCELURL", Value = "http://localhost:53744/cancel", Type = ParameterType.GetOrPost });

            //var result = client.Execute(request);

            return Redirect("www.google.com");
        }
    }
}
