using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.Admin;
using MyJobLeads.DomainModel.Data;
using OpenPop.Pop3;
using MyJobLeads.DomainModel.Entities.Admin;
using OpenPop.Pop3.Exceptions;

namespace MyJobLeads.DomainModel.Processes.Admin
{
    public class MyConsoleProcesses : IProcess<FetchFpContactResponseEmailsParams, GeneralSuccessResultViewModel>
    {
        protected MyJobLeadsDbContext _context;

        public MyConsoleProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Retrieves emailed FillPerfect contact us form response emails
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
    }
}
