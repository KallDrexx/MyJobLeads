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
using MyJobLeads.Paypal;
using MyJobLeads.DomainModel.Enums;
using System.Configuration;

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
            string paypalUrl = ConfigurationManager.AppSettings["PayPalRedirectUrl"];
            if (string.IsNullOrWhiteSpace(paypalUrl))
                throw new InvalidOperationException("No paypal redirect url specified");

            var order = _context.Orders
                                .Where(x => x.Id == model.OrderId && x.OrderedForId == CurrentUserId)
                                .FirstOrDefault();

            if (order == null || order.OrderStatus != DomainModel.Enums.OrderStatus.AwaitingPayment)
                return RedirectToAction(MVC.Home.Index());

            // Form long parameters
            string returnUrl = string.Concat(Request.Url.Scheme, "://", Request.Url.DnsSafeHost, ":", Request.Url.Port, Url.Action(MVC.Products.Pay.ProcessPayPal()));
            returnUrl = returnUrl.Replace(":80/", "/");

            string cancelUrl = string.Concat(Request.Url.Scheme, "://", Request.Url.DnsSafeHost, ":", Request.Url.Port, Url.Action(MVC.Products.Pay.Index(model.OrderId)));
            cancelUrl = cancelUrl.Replace(":80/", "/");

            // Start the workflow for paypal
            string tokenResponse = GetPaypalTransactionToken(order.TotalPrice, returnUrl, cancelUrl);

            if (string.IsNullOrWhiteSpace(tokenResponse))
                throw new InvalidOperationException("No paypal token was received");

            tokenResponse = HttpUtility.UrlDecode(tokenResponse).Replace("TOKEN=", "");
            order.PayPalToken = tokenResponse;
            _context.SaveChanges();

            // Redirect to paypal with the token
            paypalUrl = string.Concat(paypalUrl, "?cmd=_express-checkout&useraction=commit&token=", HttpUtility.UrlEncode(tokenResponse));
            return Redirect(paypalUrl);
        }

        public virtual ActionResult ProcessPayPal(string token, string payerId)
        {
            var order = _context.Orders
                                .Where(x => x.PayPalToken == token && x.OrderedForId == CurrentUserId)
                                .FirstOrDefault();

            if (order == null || order.OrderStatus != DomainModel.Enums.OrderStatus.AwaitingPayment)
                return RedirectToAction(MVC.Home.Index());

            // Perform the payment
            string transactionId = ProcessPaypalTransaction(token, payerId, order.TotalPrice);
            order.PayPalTransactionId = transactionId;
            order.OrderStatus = OrderStatus.Completed;
            _context.SaveChanges();

            var utils = new OrderUtils(_context, CurrentUserId);
            utils.ActivateOrderedLicenses(order);
            string license = utils.GetLicenseDescription(order.FillPerfectLicenses.First().LicenseType);

            return RedirectToAction(MVC.Products.FillPerfect.LicenseActivated(
                (Guid)_context.Users.Find(order.OrderedForId).FillPerfectKey,
                order.FillPerfectLicenses.First().EffectiveDate,
                order.FillPerfectLicenses.First().ExpirationDate,
                license));
        }

        protected string GetPaypalTransactionToken(decimal price, string returnUrl, string cancelUrl)
        {
            var client = new PayPalAPIAAInterfaceClient();
            var credentials = GetPaypalCredentials();
            var request = new SetExpressCheckoutReq
            {
                SetExpressCheckoutRequest = new SetExpressCheckoutRequestType
                {
                    Version = "89.0",
                    SetExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType
                    {
                        PaymentAction = PaymentActionCodeType.Sale,
                        OrderTotal = new BasicAmountType { Value = price.ToString(), currencyID = CurrencyCodeType.USD },
                        PaymentActionSpecified = true,
                        ReturnURL = returnUrl,
                        CancelURL = cancelUrl
                    }
                }
            };

            var response = client.SetExpressCheckout(ref credentials, request);
            if (response.Ack == AckCodeType.Failure)
                throw new InvalidOperationException("Paypal returned the following error: " + response.Errors.FirstOrDefault().LongMessage);

            return response.Token;
        }

        protected string ProcessPaypalTransaction(string token, string payerId, decimal total)
        {
            var client = new PayPalAPIAAInterfaceClient();
            var credentials = GetPaypalCredentials();
            var request = new DoExpressCheckoutPaymentReq
            {
                DoExpressCheckoutPaymentRequest = new DoExpressCheckoutPaymentRequestType
                {
                    Version = "89.0",
                    DoExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType
                    {
                        Token = token,
                        PayerID = payerId,
                        PaymentAction = PaymentActionCodeType.Sale,
                        PaymentDetails = new PaymentDetailsType[]
                        {
                            new PaymentDetailsType
                            {
                                OrderTotal = new BasicAmountType { Value = total.ToString(), currencyID = CurrencyCodeType.USD }
                            }
                        }
                    }
                }
            };
            var response = client.DoExpressCheckoutPayment(ref credentials, request);

            if (response.Ack == AckCodeType.Failure)
                throw new InvalidOperationException("Paypal returned the following error: " + response.Errors.FirstOrDefault().LongMessage);

            if (response.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.Count() == 0)
                throw new InvalidOperationException("No payment transaction returned from paypal");

            if (string.IsNullOrWhiteSpace(response.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.First().TransactionID))
                throw new InvalidOperationException("No payment transaction ID returned");

            return response.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.First().TransactionID;
        }

        protected CustomSecurityHeaderType GetPaypalCredentials()
        {
            string username = ConfigurationManager.AppSettings["PayPalUsername"];
            string password = ConfigurationManager.AppSettings["PayPalPassword"];
            string signature = ConfigurationManager.AppSettings["PayPalApiSignature"];

            if (username == null)
                throw new InvalidOperationException("No paypal username specified");

            if (password == null)
                throw new InvalidOperationException("No paypal password specified");

            if (signature == null)
                throw new InvalidOperationException("No paypal api secret specified");

            return new CustomSecurityHeaderType
            {
                Credentials = new UserIdPasswordType
                {
                    Username = username,
                    Password = password,
                    Signature = signature
                }
            };
        }
    }
}
