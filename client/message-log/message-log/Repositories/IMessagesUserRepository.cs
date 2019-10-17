using System;
using message_log.Models;

namespace message_log.Repositories
{
    public interface IMessagesUserRepository
    {
        MessagesUser GetByUserName(string userName);
        MessagesUser Save(MessagesUser user);
    }
}
