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

        public int CopyMessages(int fromEventID, int toEventID)
        {
            if (fromEventID == toEventID)
            {
                throw new Exception("Can't copy to same event");
            }
            var messagesToCopy = this._messageContext.Message.Where(m => m.EventID == fromEventID &&
            m.ApprovalID != 1).ToList();

            var copiedMessages = messagesToCopy.Select(m =>
            {
                var c = m.Copy();
                c.MessageID = 0;
                c.EventID = toEventID;
                return c;
            }).ToList();

            foreach (var msg in copiedMessages)
            {
                this._messageContext.Message.Add(msg);
                this._messageContext.SaveChanges();
            }

            this._messageContext.SaveChanges();

            return copiedMessages.Count();
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

        public IEnumerable<string> GetProposedRecipients(int eventID)
        {
            var messages = this.GetAllByEventID(eventID);
            return messages.Select(msg => msg.Recipient).Where(r => !String.IsNullOrEmpty(r))
                .Distinct()
                .OrderBy(r => r);
        }

        public IEnumerable<string> GetProposedSenders(int eventID)
        {
            var messages = this.GetAllByEventID(eventID);
            return messages.Select(msg => msg.Sender).Where(s => !String.IsNullOrEmpty(s))
                .Distinct()
                .OrderBy(s => s);
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
