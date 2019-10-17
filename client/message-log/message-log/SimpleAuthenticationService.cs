using System;
using System.Collections.Generic;
using message_log.Models;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using message_log.Repositories;
using Microsoft.Extensions.Logging;

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
            },
            new User
            {
                UserName = "cedric",
                PasswordHash = "+NR+9mBWdxy9PZt85AzTuxx7jq/os4Atcf5pm2I8ShE="
            },
            new User
            {
                UserName = "suivi",
                PasswordHash = "quch3TEC1UWwIjw6mCJa9D7IEwlpi/cAp9Kd5UcP/jc="
            }
        };

        private readonly IMessagesUserRepository _messagesUserRepository;
        private readonly ILogger _logger;
        public SimpleAuthenticationService(IMessagesUserRepository messagesUserRepository,
            ILogger<SimpleAuthenticationService> logger)
        {
            this._messagesUserRepository = messagesUserRepository;
            this._logger = logger;
        }

        public bool IsAuthenticated(string username, string password)
        {
            var correspondingUser = AuthorizedUsers.FirstOrDefault(u =>
            u.UserName == username);

            var dbUser = this._messagesUserRepository.GetByUserName(username);
            if (dbUser == null)
            {
                this._logger.Log(LogLevel.Warning, "Did not find a user matching " +
                    username);
            }
            else
            {
                this._logger.Log(LogLevel.Warning, "Found a user with ID " +
                    dbUser.MessagesUserID + " and name " + dbUser.UserName);
            }

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
