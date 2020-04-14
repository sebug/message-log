using System;
using System.Collections.Generic;
using message_log.Models;

namespace message_log.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetAllByEventID(int eventID);
        IEnumerable<string> GetProposedSenders(int eventID);
        IEnumerable<string> GetProposedRecipients(int eventID);
        Message Save(Message message);
        void Delete(int messageID);
    }
}
