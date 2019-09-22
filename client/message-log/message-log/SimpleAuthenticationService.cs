using System;
using System.Collections.Generic;
using message_log.Models;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace message_log
{
    /// <summary>
    /// Embarrassingly trivial auth implementation. The goal is to just withstand the most
    /// trivial attempts of unauthenticated access. I evaluated whether I could do this
    /// properly, but for next week it'll do
    /// </summary>
    public class SimpleAuthenticationService : IAuthenticationService
    {
        public static List<User> AuthorizedUsers = new List<User>
        {
            new User
            {
                UserName = "sebastian",
                PasswordHash = "47aXRSLKE8uQ168xk9v2Ranq5Kb1RhBp3CEVFeiqCv4="
            }
        };

        public bool IsAuthenticated(string username, string password)
        {
            var correspondingUser = AuthorizedUsers.FirstOrDefault(u =>
            u.UserName == username);
            if (correspondingUser == null)
            {
                return false;
            }
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            using (var mySha256 = SHA256.Create())
            {
                byte[] hashValue = mySha256.ComputeHash(passwordBytes);
                string hashString = Convert.ToBase64String(hashValue);
                if (hashString == correspondingUser.PasswordHash)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
