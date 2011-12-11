using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.PositionSearching;
using MyJobLeads.DomainModel.ProcessParams.PositionSearching.LinkedIn;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using MyJobLeads.DomainModel.LibSupport.DotNetOpenAuth;
using System.Net;
using System.Web;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Enums;
using System.Xml;
using System.Xml.Linq;
using MyJobLeads.DomainModel.Exceptions.OAuth;
using MyJobLeads.DomainModel.Processes.OAuth;
using MyJobLeads.DomainModel.Commands.Companies;
using System.Transactions;
using MyJobLeads.DomainModel.ViewModels.Positions;
using MyJobLeads.DomainModel.ProcessParams.Positions;
using MyJobLeads.DomainModel.Queries.Companies;

namespace MyJobLeads.DomainModel.Processes.PositionSearching
{
    public class LinkedInPositionSearchProcesses 
        : IProcess<LinkedInPositionSearchParams, PositionSearchResultsViewModel>,
          IProcess<LinkedInPositionDetailsParams, ExternalPositionDetailsViewModel>,
          IProcess<AddLinkedInPositionParams, ExternalPositionAddedResultViewModel>
    {
        protected MyJobLeadsDbContext _context;
        protected IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> _verifyLiTokenProcess;
        protected CreateCompanyCommand _createCompanyCmd;
        protected IProcess<CreatePositionParams, PositionDisplayViewModel> _createPositionProcess;
        protected CompanyByIdQuery _companyQuery;

        public LinkedInPositionSearchProcesses(MyJobLeadsDbContext context, 
                            IProcess<VerifyUserLinkedInAccessTokenParams, UserAccessTokenResultViewModel> verifyLiTokenProcess,
                            CreateCompanyCommand createCompanyCmd,
                            IProcess<CreatePositionParams, PositionDisplayViewModel> createPositionProcess,
                            CompanyByIdQuery companyQuery)
        {
            _context = context;
            _verifyLiTokenProcess = verifyLiTokenProcess;
            _createCompanyCmd = createCompanyCmd;
            _createPositionProcess = createPositionProcess;
            _companyQuery = companyQuery;
        }

        /// <summary>
        /// Executes the linked in position search for the user 
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public PositionSearchResultsViewModel Execute(LinkedInPositionSearchParams procParams)
        {
            const int resultsPageSize = 10;

            // Get the user's access token
            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();

            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            if (!_verifyLiTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = user.Id }).AccessTokenValid)
                throw new UserHasNoValidOAuthAccessTokenException(user.Id, TokenProvider.LinkedIn);

            // Form the API Url based on the criteria
            string apiUrl = string.Format("http://api.linkedin.com/v1/job-search?country-code={0}&keywords={1}&start={2}&count={3}", 
                                HttpUtility.UrlEncode(procParams.CountryCode), 
                                HttpUtility.UrlEncode(procParams.Keywords), 
                                procParams.ResultsPageNum * resultsPageSize,
                                resultsPageSize);

            if (!string.IsNullOrWhiteSpace(procParams.ZipCode))
                apiUrl += "&postal-code=" + procParams.ZipCode;

            // Perform the search
            var consumer = new WebConsumer(LinkedInOAuthProcesses.GetLiDescription(), new LinkedInTokenManager(_context));
            var endpoint = new MessageReceivingEndpoint(apiUrl, HttpDeliveryMethods.GetRequest);
            var request = consumer.PrepareAuthorizedRequest(endpoint, user.LinkedInOAuthData.Token);
            var response = request.GetResponse();

            // Get the results from the respones
            var xmlResponse = XDocument.Load(response.GetResponseStream());
            var resultsVm = new PositionSearchResultsViewModel 
            { 
                PageNum = procParams.ResultsPageNum, 
                DataSource = ExternalDataSource.LinkedIn,
                PageSize = resultsPageSize
            };

            resultsVm.Results = (from job in xmlResponse.Descendants("job")
                                 select new PositionSearchResultsViewModel.PositionSearchResultViewModel
                                 {
                                     JobId = job.Element("id").Value,
                                     Company = job.Element("company").Element("name").Value,
                                     Location = job.Element("location-description").Value,
                                     Description = job.Element("description-snippet").Value
                                 }).ToList();

            var searchStats = xmlResponse.Descendants("jobs").First();
            resultsVm.TotalCount = Convert.ToInt32(searchStats.Attribute("total").Value);

            if (searchStats.Attribute("start") != null)
                resultsVm.PageNum = Convert.ToInt32(searchStats.Attribute("start").Value) / resultsPageSize;
            else
                resultsVm.PageNum = 0;
            
            //resultsVm.Results = xmlResponse.wh
            return resultsVm;
        }

        /// <summary>
        /// Retrieves the detail of the specified position from Linked In
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalPositionDetailsViewModel Execute(LinkedInPositionDetailsParams procParams)
        {
            // Get the user's access token
            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();

            return GetPositionDetails(procParams.PositionId.ToString(), user.LinkedInOAuthData.Token);
        }

        /// <summary>
        /// Adds the specified Linked In position (and associated company) to the user's current job search
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public ExternalPositionAddedResultViewModel Execute(AddLinkedInPositionParams procParams)
        {
            Company company;
            PositionDisplayViewModel position;

            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.LinkedInOAuthData)
                               .SingleOrDefault();

            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            // Make sure we have a valid oauth token for the user
            if (!_verifyLiTokenProcess.Execute(new VerifyUserLinkedInAccessTokenParams { UserId = user.Id }).AccessTokenValid)
                throw new UserHasNoValidOAuthAccessTokenException(user.Id, TokenProvider.LinkedIn);

            // Retrieve the position details
            var positionDetails = GetPositionDetails(procParams.PositionId, user.LinkedInOAuthData.Token);
            if (positionDetails == null)
                throw new MJLEntityNotFoundException(typeof(ExternalPositionDetailsViewModel), procParams.PositionId);

            // Start a transaction to make sure all operations can be rolled back
            using (var transaction = new TransactionScope())
            {
                if (procParams.CreateNewCompany)
                {
                    // Create the new company
                    company = _createCompanyCmd.SetName(positionDetails.CompanyName)
                                               .WithJobSearch(Convert.ToInt32(user.LastVisitedJobSearchId))
                                               .CalledByUserId(procParams.RequestingUserId)
                                               .Execute();
                }
                else
                {
                    company = _companyQuery.WithCompanyId(procParams.ExistingCompanyId)
                                           .RequestedByUserId(user.Id)
                                           .Execute();
                    if (company == null)
                        throw new MJLEntityNotFoundException(typeof(Company), procParams.ExistingCompanyId);
                }

                // Setup the notes field
                string notes = string.Format("Location: {1}{0}Job Type: {2}{0}Experience Level: {3}{0}Job Functions: {4}{0}",
                                    Environment.NewLine,
                                    positionDetails.Location,
                                    positionDetails.JobType,
                                    positionDetails.ExperienceLevel,
                                    positionDetails.JobFunctions);

                // Create the position
                position = _createPositionProcess.Execute(new CreatePositionParams
                {
                    RequestingUserId = procParams.RequestingUserId,
                    CompanyId = company.Id,
                    Title = positionDetails.Title,
                    Notes = notes,
                    LinkedInId = procParams.PositionId
                });

                transaction.Complete();

                return new ExternalPositionAddedResultViewModel
                {
                    PositionId = position.Id,
                    PositionTitle = position.Title,
                    CompanyId = company.Id,
                    CompanyName = company.Name
                };
            }   
        }

        protected ExternalPositionDetailsViewModel GetPositionDetails(string positionId, string userOAuthToken)
        {
            // Form the API Url based on the criteria
            string apiUrl =
                string.Format(
                    "http://api.linkedin.com/v1/jobs/{0}:(id,company:(id,name),position:({1}),description,posting-date,active,job-poster:(id,first-name,last-name,headline))",
                    positionId,
                    "title,location,job-functions,industries,job-type,experience-level");

            // Perform the search
            var consumer = new WebConsumer(LinkedInOAuthProcesses.GetLiDescription(), new LinkedInTokenManager(_context));
            var endpoint = new MessageReceivingEndpoint(apiUrl, HttpDeliveryMethods.GetRequest);
            var request = consumer.PrepareAuthorizedRequest(endpoint, userOAuthToken);

            WebResponse response;
            try { response = request.GetResponse(); }
            catch (WebException ex)
            {
                // If the response is a bad request, that means no id exists for the specified job id
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.BadRequest)
                    return null;

                throw;
            }

            // Get the results
            var result = new ExternalPositionDetailsViewModel { DataSource = ExternalDataSource.LinkedIn };
            var xmlResponse = XDocument.Load(response.GetResponseStream());
            var job = xmlResponse.Descendants("job").First();
            var position = job.Element("position");

            result.Id = job.Element("id").Value;
            //result.CompanyId = job.Element("company").Element("id").Value;
            result.CompanyName = job.Element("company").Element("name").Value;
            result.Description = job.Element("description").Value;
            result.ExperienceLevel = position.Element("experience-level").Element("name").Value;
            result.JobType = position.Element("job-type").Element("name").Value;
            result.Location = position.Element("location").Element("name").Value;
            result.Title = position.Element("title").Value;
            result.IsActive = Convert.ToBoolean(job.Element("active").Value);

            result.Industries = position.Descendants("industries")
                                        .Select(x => x.Element("industry").Element("name").Value)
                                        .Aggregate((cur, next) => cur + ", " + next);

            result.JobFunctions = position.Descendants("job-functions")
                                          .Select(x => x.Element("job-function").Element("name").Value)
                                          .Aggregate((current, next) => current + ", " + next);

            var postingDate = job.Element("posting-date");
            int year = Convert.ToInt32(postingDate.Element("year").Value);
            int month = Convert.ToInt32(postingDate.Element("month").Value);
            int day = Convert.ToInt32(postingDate.Element("day").Value);
            result.PostedDate = new DateTime(year, month, day);

            var poster = job.Element("job-poster");
            result.JobPosterId = poster.Element("id").Value;
            result.JobPosterName = string.Format("{0} {1}", poster.Element("first-name").Value, poster.Element("last-name").Value);
            result.JobPosterHeadline = poster.Element("headline").Value;

            return result;
        }
    }
}