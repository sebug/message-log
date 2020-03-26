using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IAuthenticationService _authenticationService;
        public LogStreamModel(IMessageRepository messageRepository,
            IEventRepository eventRepository,
            IAuthenticationService authenticationService)
        {
            this._messageRepository = messageRepository;
            this._eventRepository = eventRepository;
            this._authenticationService = authenticationService;
        }

        public List<Message> Messages { get; set; }

        public string EventName { get; set; }

        private bool CheckAuthentication()
        {
            string username = this.Request.Cookies["username"];
            string password = this.Request.Cookies["password"];

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                return false;
            }
            var passwordBytes = Convert.FromBase64String(password);
            password = Encoding.UTF8.GetString(passwordBytes);

            return this._authenticationService.IsAuthenticated(username, password);
        }

        public IActionResult OnGet(int eventID)
        {
            if (!this.CheckAuthentication())
            {
                return this.Redirect("/Login?eventID=" + eventID);
            }

            var msgs = this._messageRepository.GetAllByEventID(eventID);
            msgs = msgs.OrderByDescending(msg => msg.EnteredOn);
            this.Messages = msgs.ToList();

            var ev = this._eventRepository.GetAll().FirstOrDefault(e => e.EventID == eventID);
            if (ev != null)
            {
                this.EventName = ev.EventName;
                this.ViewData["Title"] = this.EventName;
            }

            return null;
        }
    }
}
