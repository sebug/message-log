using System;
using System.Collections.Generic;
using message_log.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace message_log.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageContext _messageContext;
        public MessageRepository(MessageContext messageContext)
        {
            this._messageContext = messageContext;
        }

        public void Delete(int messageID)
        {
            var messageToDelete = this._messageContext.Message.FirstOrDefault(m => m.MessageID == messageID);
            if (messageToDelete != null)
            {
                this._messageContext.Entry(messageToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            this._messageContext.SaveChanges();
        }

        public IEnumerable<Message> GetAllByEventID(int eventID)
        {
            return this._messageContext.Message.Where(m => m.EventID == eventID)
                .Include(m => m.Priority)
                .Include(m => m.Approval)
                .ToList();
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
                this._messageContext.Entry(message).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
