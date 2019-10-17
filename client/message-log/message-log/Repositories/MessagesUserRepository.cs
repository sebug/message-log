using System;
using System.Linq;
using message_log.Models;
using Microsoft.EntityFrameworkCore;

namespace message_log.Repositories
{
    public class MessagesUserRepository : IMessagesUserRepository
    {
        private readonly MessagesUserContext _messagesUserContext;
        public MessagesUserRepository(MessagesUserContext messagesUserContext)
        {
            this._messagesUserContext = messagesUserContext;
        }

        public MessagesUser GetByUserName(string userName)
        {
            return this._messagesUserContext.MessagesUser.FirstOrDefault(u => u.UserName == userName);
        }

        public MessagesUser Save(MessagesUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.MessagesUserID >= 0)
            {
                this._messagesUserContext.MessagesUser.Attach(user);
                this._messagesUserContext.Entry(user).State = EntityState.Modified;
            }
            else
            {
                this._messagesUserContext.MessagesUser.Add(user);
            }
            this._messagesUserContext.SaveChanges();
            return user;
        }
    }
}
