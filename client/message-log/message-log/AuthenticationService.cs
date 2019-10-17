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
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IMessagesUserRepository _messagesUserRepository;
        private readonly ILogger _logger;
        public AuthenticationService(IMessagesUserRepository messagesUserRepository,
            ILogger<AuthenticationService> logger)
        {
            this._messagesUserRepository = messagesUserRepository;
            this._logger = logger;
        }

        public bool ChangePassword(string username, string newPassword)
        {
            var correspondingUser = this._messagesUserRepository.GetByUserName(username);

            if (correspondingUser == null)
            {
                return false;
            }

            var passwordBytes = Encoding.UTF8.GetBytes(newPassword);
            using (var mySha256 = SHA256.Create())
            {
                byte[] hashValue = mySha256.ComputeHash(passwordBytes);
                correspondingUser.PasswordHash = Convert.ToBase64String(hashValue);
            }
            correspondingUser = this._messagesUserRepository.Save(correspondingUser);
            return true;
        }

        public bool IsAuthenticated(string username, string password)
        {
            var correspondingUser = this._messagesUserRepository.GetByUserName(username);

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
