using System;
using System.Collections.Generic;
using message_log.Models;
using System.Linq;

namespace message_log.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageContext _messageContext;
        public MessageRepository(MessageContext messageContext)
        {
            this._messageContext = messageContext;
        }

        public IEnumerable<Message> GetAllByEventID(int eventID)
        {
            return this._messageContext.Message.Where(m => m.EventID == eventID).ToList();
        }

        public Message Save(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (message.MessageID > 0)
            {
                this._messageContext.Message.Attach(message);
            }
            else
            {
                this._messageContext.Message.Add(message);
            }
            this._messageContext.SaveChanges();
            return message;
        }
    }
}
