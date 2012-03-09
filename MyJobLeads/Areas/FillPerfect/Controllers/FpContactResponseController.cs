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

namespace MyJobLeads.Areas.FillPerfect.Controllers
{
    [RequiresSiteAdmin]
    public partial class FpContactResponseController : MyJobLeadsBaseController
    {
        protected IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> _fetchResponseProc;
        protected IProcess<GetFpContactResponsesParams, SearchResultsViewModel<FillPerfectContactResponse>> _getDbFpContactResponseProc;

        public FpContactResponseController(MyJobLeadsDbContext context, 
                                            IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel> fetchResponseProc,
                                            IProcess<GetFpContactResponsesParams, SearchResultsViewModel<FillPerfectContactResponse>> getDbFpContactResponseProc)
        {
            _context = context;
            _fetchResponseProc = fetchResponseProc;
            _getDbFpContactResponseProc = getDbFpContactResponseProc;
        }

        public virtual ActionResult Index()
        {
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
    }
}
