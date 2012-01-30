using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.ExternalAuth;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuthorization.Jigsaw;
using MyJobLeads.DomainModel.ProcessParams.ContactSearching.Jigsaw;
using MyJobLeads.DomainModel.ViewModels.ContactSearching;
using MyJobLeads.DomainModel.Exceptions.Jigsaw;
using RestSharp;
using MyJobLeads.DomainModel.Processes.ExternalAuth;
using System.Net;
using Newtonsoft.Json;
using MyJobLeads.DomainModel.Json.Jigsaw;
using AutoMapper;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Commands.Companies;
using System.Transactions;
using MyJobLeads.DomainModel.Queries.Companies;
using MyJobLeads.DomainModel.Commands.Contacts;
using MyJobLeads.DomainModel.Json.Converters;
using MyJobLeads.DomainModel.Processes.CompanySearching;

namespace MyJobLeads.DomainModel.Processes.ContactSearching
{
    public class JigsawContactSearchProcesses : IProcess<JigsawContactSearchParams, ExternalContactSearchResultsViewModel>,
                                                IProcess<AddJigsawContactToJobSearchParams, ExternalContactAddedResultViewModel>,
                                                IProcess<GetJigsawContactDetailsParams, ExternalContactSearchResultsViewModel.ContactResultViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<GetUserJigsawCredentialsParams, JigsawCredentialsViewModel> _getJigsawCredentialsProc;
        protected CreateCompanyCommand _createCompanyCmd;
        protected CompanyByIdQuery _companyByIdQuery;
        protected CreateContactCommand _createContactCmd;

        public JigsawContactSearchProcesses(MyJobLeadsDbContext context, 
                                            IProcess<GetUserJigsawCredentialsParams, JigsawCredentialsViewModel> getJsCredProc,
                                            CreateCompanyCommand createCompanyCmd,
                                            CompanyByIdQuery compByIdQuery,
                                            CreateContactCommand createContactcmd)
        {
            _context = context;
            _getJigsawCredentialsProc = getJsCredProc;
            _createCompanyCmd = createCompanyCmd;
            _companyByIdQuery = compByIdQuery;
            _createContactCmd = createContactcmd;
        }

        /// <summary>
        /// Performs a jigsaw search on the user's behalf
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalContactSearchResultsViewModel Execute(JigsawContactSearchParams procParams)
        {
            const int PAGE_SIZE = 50;
            var credentials = JigsawAuthProcesses.GetMyLeadsAccountCredentials();

            // Perform the API query
            var client = new RestClient("https://www.jigsaw.com/");
            var request = new RestRequest("rest/searchContact.json", Method.GET);
            request.AddParameter("token", JigsawAuthProcesses.GetAuthToken());
            request.AddParameter("username", credentials.JigsawUsername);
            request.AddParameter("password", credentials.JigsawPassword);
            request.AddParameter("firstname", procParams.FirstName);
            request.AddParameter("lastname", procParams.LastName);
            request.AddParameter("levels", procParams.Levels);
            request.AddParameter("companyName", procParams.CompanyName);
            request.AddParameter("title", procParams.Title);
            request.AddParameter("industry", procParams.Industry);
            request.AddParameter("departments", procParams.Departments);
            request.AddParameter("offset", procParams.RequestedPageNum * PAGE_SIZE);
            request.AddParameter("pageSize", PAGE_SIZE);
            var response = client.Execute(request);

            // Check for a forbidden result
            if (response.StatusCode == HttpStatusCode.Forbidden)
                JigsawAuthProcesses.ThrowInvalidResponse(response.Content, procParams.RequestingUserId, response.ErrorMessage);

            // Get the returned json and convert it to the view model
            var json = JsonConvert.DeserializeObject<ContactSearchResponseJson>(response.Content, new JigsawDateTimeConverter());
            var model = Mapper.Map<ContactSearchResponseJson, ExternalContactSearchResultsViewModel>(json);
            //model.Results = new List<ExternalContactSearchResultsViewModel.ContactResultViewModel>();
            //foreach (var contact in json.Contacts)
            //    model.Results.Add(Mapper.Map<ContactDetailsJson, ExternalContactSearchResultsViewModel.ContactResultViewModel>(contact));

            model.PageSize = PAGE_SIZE;
            model.DisplayedPageNumber = procParams.RequestedPageNum;

            return model;
        }

        /// <summary>
        /// Adds a jigsaw contact to the user's job search
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalContactAddedResultViewModel Execute(AddJigsawContactToJobSearchParams procParams)
        {
            bool jigsawContactOwned = true;
            var credentials = JigsawAuthProcesses.GetMyLeadsAccountCredentials();

            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LastVisitedJobSearch)
                               .FirstOrDefault();
            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            // If we are using an existing company, make sure it exists in the user's job search
            if (!procParams.CreateCompanyFromJigsaw)
                if (!_context.Companies.Any(x => x.Id == procParams.ExistingCompanyId && x.JobSearch.Id == user.LastVisitedJobSearchId))
                    throw new MJLEntityNotFoundException(typeof(Company), procParams.ExistingCompanyId);

            // Perform the API query to get the jigsaw contact only if the user has access to the contact
            var client = new RestClient("https://www.jigsaw.com/");
            string contactUrl = string.Format("rest/contacts/{0}.json", procParams.JigsawContactId);
            var request = new RestRequest(contactUrl, Method.GET);
            request.AddParameter("token", JigsawAuthProcesses.GetAuthToken());
            request.AddParameter("username", credentials.JigsawUsername);
            request.AddParameter("password", credentials.JigsawPassword);
            request.AddParameter("purchaseFlag", false);
            var response = client.Execute(request);

            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    JigsawAuthProcesses.ThrowInvalidResponse(response.Content, procParams.RequestingUserId, procParams.JigsawContactId);
            }
            catch (JigsawContactNotOwnedException)
            {
                jigsawContactOwned = false;
            }

            // If the user owns the contact, update the info from Jigsaw
            if (jigsawContactOwned)
            {
                // Convert the json string into an object and evaluate it
                var json = JsonConvert.DeserializeObject<ContactDetailsResponseJson>(response.Content);
                if (json.Unrecognized.ContactId.Count > 0)
                    throw new JigsawContactNotFoundException(procParams.JigsawContactId);

                procParams.Name = json.Contacts[0].FirstName + " " + json.Contacts[0].LastName;
                procParams.Title = json.Contacts[0].Title;
                procParams.Phone = json.Contacts[0].Phone;
                procParams.Email = json.Contacts[0].Email;
            }

            // Use a transaction to prevent creating a company without the contact
            using (var transaction = new TransactionScope())
            {
                // Retrieve the company from the database, or get the information to create a company from Jigsaw
                Company company;
                if (procParams.CreateCompanyFromJigsaw)
                {
                    // Create the company
                    var jsComp = JigsawCompanySearchProcesses.GetJigsawCompany(Convert.ToInt32(procParams.JigsawCompanyId), procParams.RequestingUserId);
                    company = _createCompanyCmd.CalledByUserId(procParams.RequestingUserId)
                                               .WithJobSearch((int)user.LastVisitedJobSearchId)
                                               .SetName(jsComp.name)
                                               .SetIndustry(jsComp.industry1)
                                               .SetCity(jsComp.city)
                                               .SetState(jsComp.state)
                                               .SetZip(jsComp.zip)
                                               .SetPhone(jsComp.phone)
                                               .SetJigsawId(Convert.ToInt32(jsComp.companyId))
                                               .Execute();
                }

                // Retrieve the company from the database
                else
                {
                    // Already verified the company exists in the database, so no need for null checking
                    company = _companyByIdQuery.RequestedByUserId(procParams.RequestingUserId)
                                               .WithCompanyId(procParams.ExistingCompanyId)
                                               .Execute();
                }

                // Create the contact
                var contact = _createContactCmd.RequestedByUserId(procParams.RequestingUserId)
                                               .WithCompanyId(company.Id)
                                               .SetName(procParams.Name)
                                               .SetTitle(procParams.Title)
                                               .SetDirectPhone(procParams.Phone)
                                               .SetEmail(procParams.Email)
                                               .SetJigsawId(Convert.ToInt32(procParams.JigsawContactId))
                                               .SetHasJigsawAccess(jigsawContactOwned)
                                               .Execute();

                transaction.Complete();

                return new ExternalContactAddedResultViewModel
                {
                    CompanyId = company.Id,
                    CompanyName = company.Name,
                    ContactId = contact.Id,
                    ContactName = contact.Name
                };
            }
        }

        /// <summary>
        /// Retrieves the details for the specified Jigsaw contact
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalContactSearchResultsViewModel.ContactResultViewModel Execute(GetJigsawContactDetailsParams procParams)
        {
            var credentials = _getJigsawCredentialsProc.Execute(new GetUserJigsawCredentialsParams { RequestingUserId = procParams.RequestingUserId });
            if (credentials == null)
                throw new JigsawCredentialsNotFoundException(procParams.RequestingUserId);

            // Perform the API query to get the jigsaw contact only if the user has access to the contact
            var client = new RestClient("https://www.jigsaw.com/");
            string contactUrl = string.Format("rest/contacts/{0}.json", procParams.JigsawContactId);
            var request = new RestRequest(contactUrl, Method.GET);
            request.AddParameter("token", JigsawAuthProcesses.GetAuthToken());
            request.AddParameter("username", credentials.JigsawUsername);
            request.AddParameter("password", credentials.JigsawPassword);
            request.AddParameter("purchaseFlag", procParams.PurchaseContact);
            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                JigsawAuthProcesses.ThrowInvalidResponse(response.Content, procParams.RequestingUserId, procParams.JigsawContactId.ToString());

            // Convert the json string into an object and evaluate it
            var json = JsonConvert.DeserializeObject<ContactDetailsResponseJson>(response.Content, new JigsawDateTimeConverter());
            if (json.Unrecognized.ContactId.Count > 0)
                throw new JigsawContactNotFoundException(procParams.JigsawContactId);

            return Mapper.Map<ContactDetailsJson, ExternalContactSearchResultsViewModel.ContactResultViewModel>(json.Contacts[0]);
        }
    }
}