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
    public class LogStreamModel : PageModel
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IEventRepository _eventRepository;
        public LogStreamModel(IMessageRepository messageRepository,
            IEventRepository eventRepository)
        {
            this._messageRepository = messageRepository;
            this._eventRepository = eventRepository;
        }

        public List<Message> Messages { get; set; }

        public string EventName { get; set; }

        public void OnGet(int eventID)
        {
            var msgs = this._messageRepository.GetAllByEventID(eventID);
            msgs = msgs.OrderByDescending(msg => msg.EnteredOn);
            this.Messages = msgs.ToList();

            var ev = this._eventRepository.GetAll().FirstOrDefault(e => e.EventID == eventID);
            if (ev != null)
            {
                this.EventName = ev.EventName;
                this.ViewData["Title"] = this.EventName;
            }
        }
    }
}
