using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Utilities;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.ExternalAuth;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuthorization.Jigsaw;
using System.Configuration;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.ProcessParams.ExternalAuth.Jigsaw;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Exceptions;
using MyJobLeads.DomainModel.ViewModels.JobSearches;
using MyJobLeads.DomainModel.Exceptions.Jigsaw;
using RestSharp;
using System.Net;

namespace MyJobLeads.DomainModel.Processes.ExternalAuth
{
    public class JigsawAuthProcesses : IProcess<GetUserJigsawCredentialsParams, JigsawCredentialsViewModel>,
                                       IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel>,
                                       IProcess<GetJigsawUserPointsParams, JigsawUserPointsViewModel>
    {
        protected const string KEY_APPSETTINGS = "JigsawKey";
        protected const string IV_APPSETTINGS = "JigsawIV";
        protected const string TOKEN_APPSETTINGS = "JigsawToken";

        protected MyJobLeadsDbContext _context;
        protected EncryptionUtils _encryptionUtils;

        public JigsawAuthProcesses(MyJobLeadsDbContext context, EncryptionUtils encryptionUtils)
        {
            _context = context;
            _encryptionUtils = encryptionUtils;
        }

        /// <summary>
        /// Retrieves the jigsaw credentials for the specified user
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public JigsawCredentialsViewModel Execute(GetUserJigsawCredentialsParams procParams)
        {
            // Retrieve the encryption key and IV from app.config
            byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings[KEY_APPSETTINGS]);
            byte[] iv = Convert.FromBase64String(ConfigurationManager.AppSettings[IV_APPSETTINGS]);

            // Get the user's jigsaw data
            var credentials = _context.JigsawAccountDetails
                                      .Where(x => x.AssociatedUser.Id == procParams.RequestingUserId)
                                      .FirstOrDefault();

            if (credentials == null)
                return null;

            return new JigsawCredentialsViewModel
            {
                JigsawUsername = credentials.Username,
                JigsawPassword = _encryptionUtils.DecryptStringFromBytes_AES(Convert.FromBase64String(credentials.EncryptedPassword), key, iv)
            };
        }

        /// <summary>
        /// Saves the jigsaw user credentials for the user
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public GeneralSuccessResultViewModel Execute(SaveJigsawUserCredentialsParams procParams)
        {
            // Get the encryption key and iv from the app.config
            var key = Convert.FromBase64String(ConfigurationManager.AppSettings[KEY_APPSETTINGS]);
            var iv = Convert.FromBase64String(ConfigurationManager.AppSettings[IV_APPSETTINGS]);
            var encryptedBytes = _encryptionUtils.EncryptStringToBytes_AES(procParams.JigsawPassword, key, iv);

            var user = _context.Users
                               .Where(x => x.Id == procParams.RequestingUserId)
                               .Include(x => x.JigsawAccountDetails)
                               .FirstOrDefault();

            if (user == null)
                throw new MJLEntityNotFoundException(typeof(User), procParams.RequestingUserId);

            var jigsaw = new JigsawAccountDetails
            {
                Username = procParams.JigsawUsername.Trim(),
                EncryptedPassword = Convert.ToBase64String(encryptedBytes)
            };

            user.JigsawAccountDetails = jigsaw;
            _context.SaveChanges();

            return new GeneralSuccessResultViewModel { WasSuccessful = true };
        }

        /// <summary>
        /// Retrieves the jigsaw points information for the specified user
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public JigsawUserPointsViewModel Execute(GetJigsawUserPointsParams procParams)
        {
            // Retrieve the user's credentials
            var credentials = Execute(new GetUserJigsawCredentialsParams { RequestingUserId = procParams.RequestingUserId });
            if (credentials == null)
                throw new JigsawCredentialsNotFoundException(procParams.RequestingUserId);

            // Perform the user points query
            var client = new RestClient("https://www.jigsaw.com/");
            var request = new RestRequest("rest/user.json", Method.GET);
            request.AddParameter("token", GetAuthToken());
            request.AddParameter("username", credentials.JigsawUsername);
            request.AddParameter("password", credentials.JigsawPassword);

            var response = client.Execute<JigsawUserPointsViewModel>(request);

            // If a forbidden response was given, determine if it was a bad API token or bad user credentials
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                if (response.Content.Contains("TOKEN_FAIL"))
                    throw new InvalidJigsawApiTokenException(GetAuthToken());

                if (response.Content.Contains("LOGIN_FAIL"))
                    throw new InvalidJigsawCredentialsException(procParams.RequestingUserId);

                else
                    throw new MJLException("Jigsaw request returned forbidden but was not due to a login or api token failure");
            }

            return response.Data;
        }

        /// <summary>
        /// Retrieves the jigsaw api token to use
        /// </summary>
        /// <returns></returns>
        public static string GetAuthToken()
        {
            return ConfigurationManager.AppSettings[TOKEN_APPSETTINGS];
        }
    }
}