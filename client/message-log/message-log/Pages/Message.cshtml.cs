using System;
using System.Collections.Generic;
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

        public void OnGet(int eventID)
        {
            this.Messages = this._messageRepository.GetAllByEventID(eventID).ToList();
        }

        public void OnPost(int eventID)
        {
            this.Messages = this._messageRepository.GetAllByEventID(eventID).ToList();
        }
    }
}
