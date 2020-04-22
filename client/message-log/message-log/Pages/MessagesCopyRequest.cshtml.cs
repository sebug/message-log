using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using message_log.Models;
using message_log.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace message_log.Pages
{
    public class MessagesCopyRequestModel : PageModel
    {
        [BindProperty]
        public int FromEventID { get; set; }

        [BindProperty]
        public int ToEventID { get; set; }

        public bool IsSuccess { get; set; }

        public int MessagesCopied { get; set; }

        public List<SelectListItem> Events { get; set; }

        private readonly IAuthenticationService _authenticationService;

        private readonly IEventRepository _eventRepository;

        private readonly IMessageRepository _messageRepository;

        public MessagesCopyRequestModel(IEventRepository eventRepository,
            IAuthenticationService authenticationService,
            IMessageRepository messageRepository)
        {
            this._authenticationService = authenticationService;
            this._eventRepository = eventRepository;
            this._messageRepository = messageRepository;
        }

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

        public void OnGet()
        {
            this.Events = this._eventRepository.GetAll()
                .Select(e => new SelectListItem(e.EventName, e.EventID.ToString()))
                .ToList();
        }

        public IActionResult OnPost()
        {
            if (!this.CheckAuthentication())
            {
                return this.Redirect("/Login");
            }

            this.MessagesCopied = this._messageRepository.CopyMessages(this.FromEventID, this.ToEventID);
            this.IsSuccess = true;

            return null;
        }
    }
}
