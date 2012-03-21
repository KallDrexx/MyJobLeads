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
        protected IProcess<SendFpReplyParams, GeneralSuccessResultViewModel> _sendFpReplyProc;

        public FpContactResponseController(MyJobLeadsDbContext context, 
                                            IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> fetchResponseProc,
                                            IProcess<GetFpContactResponsesParams, SearchResultsViewModel<FillPerfectContactResponse>> getDbFpContactResponseProc,
                                            IProcess<CreateDemoAccountParams, CreatedDemoAccountViewModel> createDemoActProc,
                                            IProcess<SendFpReplyParams, GeneralSuccessResultViewModel> sendFpReplyProc)
        {
            _context = context;
            _fetchResponseProc = fetchResponseProc;
            _getDbFpContactResponseProc = getDbFpContactResponseProc;
            _createDemoActProc = createDemoActProc;
            _sendFpReplyProc = sendFpReplyProc;
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

            var data = results.Results
                                .Select(x => new object[]
                                {
                                    x.Id,
                                    HttpUtility.HtmlEncode(x.Name),
                                    HttpUtility.HtmlEncode(x.Email),
                                    HttpUtility.HtmlEncode(x.School),
                                    HttpUtility.HtmlEncode(x.Program),
                                    x.ReceivedDate.ToLongDateString(),
                                    x.RepliedDate != null ? x.RepliedDate.Value.ToLongDateString() : "",
                                    string.Format("<a href=\"{0}\" class=\"inlineBlue\">Reply</a>", Url.Action(MVC.FillPerfect.FpContactResponse.CreateAccount(x.Id))),
                                    string.Format("<a href=\"{0}\" class=\"inlineBlue\">Delete</a>", Url.Action(MVC.FillPerfect.FpContactResponse.DeleteResponse(x.Id)))
                                });
            return Json(new
            {
                iTotalRecords = results.TotalResultsCount,
                aaData = data.ToArray()
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

            // Try and remove the identifiers from the response fields
            response.Email = response.Email.Replace("Email: ", "");
            response.Name = response.Name.Replace("Name: ", "");
            response.School = response.School.Replace("School: ", "");

            var model = new ContactUsCreateAccountViewModel
            {
                FpContactResponseId = responseId,
                ResponseName = response.Name,
                ResponseEmail = response.Email,
                ResponseProgram = response.Program,
                ResponseSchool = response.School,
                CreateAccountModel = new ViewModels.Admin.CreateDemoAccountViewModel
                {
                    Name = response.Name,
                    Email = response.Email,
                    OrganizationName = response.School
                }
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
            // Get the email for the current user
            var email = _context.Users
                                .Where(x => x.Id == CurrentUserId)
                                .Select(x => x.Email)
                                .Single();

            var model = new ContactUsSendReplyViewModel
            {
                ResponseId = responseId,
                ToName = toName,
                ToEmail = toEmail,
                ToOrgName = orgName,
                ToPassword = password,
                EmailContent = emailContent,
                FromAddress = email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SendReply(ContactUsSendReplyViewModel model)
        {
            // Perform validation
            if (string.IsNullOrWhiteSpace(model.EmailSubject))
                ModelState.AddModelError("EmailSubject", "Email subject cannot be empty");

            if (!ModelState.IsValid)
                return View(model);

            // Convert newlines into <br /> tags
            model.EmailContent = model.EmailContent.Replace(Environment.NewLine, "<br />");

            // Show confirmation view
            return View(MVC.FillPerfect.FpContactResponse.Views.SendReplyConfirm, model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SendReplyConfirm(ContactUsSendReplyViewModel model)
        {
            // Convert newlines into <br /> tags
            model.EmailContent = model.EmailContent.Replace(Environment.NewLine, "<br />");

            // Send the reply
            _sendFpReplyProc.Execute(new SendFpReplyParams
            {
                RequestingUserId = CurrentUserId,
                ResponseId = model.ResponseId,
                FromAddress = model.FromAddress,
                ToAddress = model.ToEmail,
                Subject = model.EmailSubject,
                EmailContent = model.EmailContent
            });
            return RedirectToAction(MVC.FillPerfect.FpContactResponse.ReplySent(model.ToName));
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult EditReply(ContactUsSendReplyViewModel model)
        {
            // Convert <br /> tags into newlines, to make editing easier
            model.EmailContent = model.EmailContent
                                      .Replace("<br />", Environment.NewLine)
                                      .Replace("<br>", Environment.NewLine);

            return View(MVC.FillPerfect.FpContactResponse.Views.SendReply, model);
        }

        public virtual ActionResult ReplySent(string toName)
        {
            var model = new ContactUsReplySentViewModel { SentToName = toName };
            return View(model);
        }
    }
}
