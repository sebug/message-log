using System;
using System.Collections.Generic;
using message_log.Models;

namespace message_log.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetAllByEventID(int eventID);
        Message Save(Message message);
    }
}
