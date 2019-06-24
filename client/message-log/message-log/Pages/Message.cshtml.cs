using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using message_log.Models;
using message_log.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace message_log.Pages
{
    public class MessageModel : PageModel
    {
        [BindProperty]
        public string EnteredOn { get; set; }

        [BindProperty]
        public string Sender { get; set; }

        [BindProperty]
        public string Recipient { get; set; }

        [BindProperty]
        public string MessageText { get; set; }

        public List<Message> Messages { get; set; }
        private readonly IMessageRepository _messageRepository;
        public MessageModel(IMessageRepository messageRepository)
        {
            this._messageRepository = messageRepository;
        }

        public void OnGet(int eventID, int? messageID = null)
        {
            this.Messages = this._messageRepository.GetAllByEventID(eventID).ToList();
            if (messageID.HasValue)
            {
                var currentMessage = this.Messages.FirstOrDefault(m => m.MessageID == messageID.Value);
                if (currentMessage != null)
                {
                    this.EnteredOn = currentMessage.EnteredOn.ToString("yyyy-MM-dd HH:mm");
                    this.Sender = currentMessage.Sender;
                    this.Recipient = currentMessage.Recipient;
                    this.MessageText = currentMessage.MessageText;
                }
            }
        }

        public IActionResult OnPost(int eventID, int? messageID = null)
        {
            DateTime dt;
            if (!String.IsNullOrEmpty(this.EnteredOn) &&
                DateTime.TryParseExact(this.EnteredOn, "yyyy-MM-dd HH:mm", new CultureInfo("fr-CH"), DateTimeStyles.AllowWhiteSpaces,
                out dt))
            {
                var message = new Message
                {
                    EnteredOn = dt,
                    EventID = eventID,
                    Sender = this.Sender,
                    Recipient = this.Recipient,
                    MessageText = this.MessageText
                };
                if (messageID.HasValue)
                {
                    message.MessageID = messageID.Value;
                }
                message = this._messageRepository.Save(message);
            }
            return this.Redirect("/Message/" + eventID);
        }
    }
}
