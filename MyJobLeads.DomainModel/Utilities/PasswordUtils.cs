using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevOne.Security.Cryptography.BCrypt;

namespace MyJobLeads.DomainModel.Utilities
{
    public class PasswordUtils
    {
        /// <summary>
        /// Generates a password hash from the credentials specified
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Hash value for the password</returns>
        public static string CreatePasswordHash(string username, string password)
        {
            // Concat username and password together, and generate a password hash from that
            return BCryptHelper.HashPassword(username + password, BCryptHelper.GenerateSalt(10));
        }

        /// <summary>
        /// Check's if the specified credentials are valid for the specified password hash value
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns>True if the credentials match the hash</returns>
        public static bool CheckPasswordHash(string username, string password, string hash)
        {
            return BCryptHelper.CheckPassword(username + password, hash);
        }

        public const int MinPasswordLength = 5;
    }
}
