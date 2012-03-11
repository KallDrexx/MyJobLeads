using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Entities.Admin;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;
using MyJobLeads.Infrastructure.Attributes;
using System.Configuration;
using MyJobLeads.Areas.FillPerfect.Models.ContactUsResponses;
using MyJobLeads.DomainModel.ViewModels.Admin;

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    [RequiresSiteAdmin]
    public partial class FpContactResponseController : MyJobLeadsBaseController
    {
        protected IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> _fetchResponseProc;
        protected IProcess<GetFpContactResponsesParams, SearchResultsViewModel<FillPerfectContactResponse>> _getDbFpContactResponseProc;
        protected IProcess<CreateDemoAccountParams, CreatedDemoAccountViewModel> _createDemoActProc;

        public FpContactResponseController(MyJobLeadsDbContext context, 
                                            IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> fetchResponseProc,
                                            IProcess<GetFpContactResponsesParams, SearchResultsViewModel<FillPerfectContactResponse>> getDbFpContactResponseProc,
                                            IProcess<CreateDemoAccountParams, CreatedDemoAccountViewModel> createDemoActProc)
        {
            _context = context;
            _fetchResponseProc = fetchResponseProc;
            _getDbFpContactResponseProc = getDbFpContactResponseProc;
            _createDemoActProc = createDemoActProc;
        }

        public virtual ActionResult Index()
        {
            // Only get new emails from the email box if running on production
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsProduction"]))
                _fetchResponseProc.Execute(new FetchFpContactResponseEmailsParams());

            return View();
        }

        public virtual JsonResult GetNewFpContactResponses()
        {
            var results = _getDbFpContactResponseProc.Execute(new GetFpContactResponsesParams
            {
                RequestingUserId = CurrentUserId,
                GetNewResponses = true
            });

            return Json(new
            {
                iTotalRecords = results.TotalResultsCount,
                aaData = results.Results
                                .Select(x => new object[]
                                {
                                    x.Id,
                                    HttpUtility.HtmlEncode(x.Name),
                                    HttpUtility.HtmlEncode(x.Email),
                                    HttpUtility.HtmlEncode(x.School),
                                    HttpUtility.HtmlEncode(x.Program),
                                    x.ReceivedDate.ToLongDateString(),
                                    x.RepliedDate != null ? x.RepliedDate.Value.ToLongDateString() : ""
                                })
                                .ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult DeleteResponse(int id)
        {
            var response = _context.FillPerfectContactResponses
                                   .Where(x => x.Id == id)
                                   .SingleOrDefault();
            if (response != null)
            {
                _context.FillPerfectContactResponses.Remove(response);
                _context.SaveChanges();
            }

            return RedirectToAction(MVC.FillPerfect.FpContactResponse.Index());
        }

        public virtual ActionResult CreateAccount(int responseId)
        {
            var response = _context.FillPerfectContactResponses
                                   .Where(x => x.Id == responseId)
                                   .SingleOrDefault();
            if (response == null)
                return RedirectToAction(MVC.FillPerfect.FpContactResponse.Index());

            var model = new ContactUsCreateAccountViewModel
            {
                FpContactResponseId = responseId,
                ResponseName = response.Name,
                ResponseEmail = response.Email,
                ResponseProgram = response.Program,
                ResponseSchool = response.School
            };

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult CreateAccount(ContactUsCreateAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _createDemoActProc.Execute(new CreateDemoAccountParams
            {
                RequestingUserID = CurrentUserId,
                Name = model.CreateAccountModel.Name,
                Email = model.CreateAccountModel.Email,
                OrganizationName = model.CreateAccountModel.OrganizationName,
                Password = model.CreateAccountModel.Password
            });

            return RedirectToAction(
                MVC.FillPerfect.FpContactResponse.SendReply(
                    model.CreateAccountModel.Name,
                    model.CreateAccountModel.Email,
                    model.CreateAccountModel.Password,
                    model.CreateAccountModel.OrganizationName,
                    model.FpContactResponseId));
        }

        public virtual ActionResult SendReply(string toName, string toEmail, string password, string orgName, int responseId, string emailContent = "")
        {
            var model = new ContactUsSendReplyViewModel
            {
                ResponseId = responseId,
                ToName = toName,
                ToEmail = toEmail,
                ToOrgName = orgName,
                ToPassword = password,
                EmailContent = emailContent
            };

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SendReply(ContactUsSendReplyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction(MVC.FillPerfect.FpContactResponse.SendReplyConfirm(model));
        }

        public virtual ActionResult SendReplyConfirm(ContactUsSendReplyViewModel model)
        {
            if (Request.HttpMethod == "GET")
                return View(model);

            else
            {
                // Send the reply

                return RedirectToAction(MVC.FillPerfect.FpContactResponse.ReplySent(model.ToName));
            }
        }

        [HttpPost]
        public virtual ActionResult EditReply(ContactUsSendReplyViewModel model)
        {
            return View(MVC.FillPerfect.FpContactResponse.Views.SendReply, model);
        }

        public virtual ActionResult ReplySent(string toName)
        {
            var model = new ContactUsReplySentViewModel { SentToName = toName };
            return View(model);
        }
    }
}
