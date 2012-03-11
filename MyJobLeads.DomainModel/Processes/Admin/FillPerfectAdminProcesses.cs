using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.Entities.EF;
using OpenPop.Pop3;
using MyJobLeads.DomainModel.Entities.Admin;
using OpenPop.Pop3.Exceptions;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;
using MyJobLeads.DomainModel.ViewModels.Authorizations;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.Exceptions;

namespace MyJobLeads.DomainModel.Processes.Admin
{
    public class FillPerfectAdminProcesses : IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel>,
                                             IProcess<GetFpContactResponsesParams, SearchResultsViewModel<FillPerfectContactResponse>>,
                                             IProcess<SendFpReplyParams, GeneralSuccessResultViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> _adminAuthProc;

        public FillPerfectAdminProcesses(MyJobLeadsDbContext context, IProcess<SiteAdminAuthorizationParams, AuthorizationResultViewModel> adminAuthProc)
        {
            _context = context;
            _adminAuthProc = adminAuthProc;
        }

        /// <summary>
        /// Retrieves emailed FillPerfect contact us form response emails and puts them in the database
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(FetchFpContactResponseEmailsParams procParams)
        {
            const string FORMS_EMAIL = "forms@interviewtools.net";
            const string FORMS_PASS = "f0rms12345";
            const string POP_HOST = "pop.gmail.com";
            const int POP_PORT = 995;
            const bool USE_SSL = true;

            // Download all the new mail messages from the email
            using (var client = new Pop3Client())
            {
                try
                {
                    client.Connect(POP_HOST, POP_PORT, USE_SSL);
                    client.Authenticate(FORMS_EMAIL, FORMS_PASS);

                    int messageCount = client.GetMessageCount();

                    for (int x = messageCount; x > 0; x--)
                    {
                        var message = client.GetMessage(x);

                        if (message.Headers.Subject == "cs fp form filled out")
                        {
                            string[] bodyLines = message.FindFirstPlainTextVersion()
                                                      .GetBodyAsText()
                                                      .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                            // make sure the message is as expected
                            if (bodyLines.Length == 4)
                            {
                                _context.FillPerfectContactResponses.Add(new FillPerfectContactResponse
                                {
                                    Name = bodyLines[0],
                                    Email = bodyLines[1],
                                    School = bodyLines[2],
                                    Program = bodyLines[3],
                                    ReceivedDate = message.Headers.DateSent
                                });
                            }
                        }
                    }

                    _context.SaveChanges();
                    client.DeleteAllMessages();
                }
                catch (PopClientException)
                {
                    return new GeneralSuccessResultViewModel
                    {
                        WasSuccessful = false
                    };
                }
            }

            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }

        /// <summary>
        /// Retrieves the FP contact us responses stored in the database
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        SearchResultsViewModel<FillPerfectContactResponse> IProcess<GetFpContactResponsesParams, SearchResultsViewModel<FillPerfectContactResponse>>.Execute(GetFpContactResponsesParams procParams)
        {
            const int PAGE_SIZE = 20;

            // Make sure the user is a site administrator
            if (!_adminAuthProc.Execute(new SiteAdminAuthorizationParams { UserId = procParams.RequestingUserId }).UserAuthorized)
                throw new UserNotAuthorizedForProcessException(procParams.RequestingUserId, typeof(GetFpContactResponsesParams), typeof(SearchResultsViewModel<FillPerfectContactResponse>));

            var query = _context.FillPerfectContactResponses
                                .Where(x => (x.RepliedDate == null) == procParams.GetNewResponses);

            return new SearchResultsViewModel<FillPerfectContactResponse>
            {
                PageSize = PAGE_SIZE,
                TotalResultsCount = query.Count(),
                Results = query.OrderByDescending(x => x.RepliedDate)
                               .ThenBy(x => x.ReceivedDate)
                               .ToList()
            };
        }

        /// <summary>
        /// Sends a FillPerfect information reply to a specific person
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(SendFpReplyParams procParams)
        {
            // Make sure the user is a site administrator
            if (!_adminAuthProc.Execute(new SiteAdminAuthorizationParams { UserId = procParams.RequestingUserId }).UserAuthorized)
                throw new UserNotAuthorizedForProcessException(procParams.RequestingUserId, typeof(SendFpReplyParams), typeof(GeneralSuccessResultViewModel));


        }
    }
}
