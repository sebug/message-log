using System;
using System.Linq;
using message_log.Models;

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
    }
}
