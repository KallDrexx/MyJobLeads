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

namespace MyJobLeads.DomainModel.Processes.ExternalAuth
{
    public class JigsawAuthProcesses : IProcess<GetUserJigsawCredentialsParams, JigsawCredentialsViewModel>,
                                       IProcess<SaveJigsawUserCredentialsParams, GeneralSuccessResultViewModel>
    {
        protected const string KEY_APPSETTINGS = "JigsawKey";
        protected const string IV_APPSETTINGS = "JigsawIV";

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
    }
}
