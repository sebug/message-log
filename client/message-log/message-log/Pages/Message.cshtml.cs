﻿using System;
using System.Collections.Generic;
using System.Globalization;
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

        [BindProperty]
        public int? PriorityID { get; set; }

        [BindProperty]
        public int? ApprovalID { get; set; }

        public List<string> ProposedSenders { get; set; }

        public List<string> ProposedRecipients { get; set; }

        public List<Message> Messages { get; set; }
        public List<SelectListItem> Priorities { get; set; }
        public List<SelectListItem> Approvals { get; set; }
        private readonly IMessageRepository _messageRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IPriorityRepository _priorityRepository;
        private readonly IApprovalRepository _approvalRepository;
        private readonly IAuthenticationService _authenticationService;
        public MessageModel(IMessageRepository messageRepository,
            IEventRepository eventRepository,
            IPriorityRepository priorityRepository,
            IApprovalRepository approvalRepository,
            IAuthenticationService authenticationService)
        {
            this._messageRepository = messageRepository;
            this._eventRepository = eventRepository;
            this._priorityRepository = priorityRepository;
            this._approvalRepository = approvalRepository;
            this._authenticationService = authenticationService;
        }

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

        public IActionResult OnGet(int eventID, int? messageID = null)
        {
            if (!this.CheckAuthentication())
            {
                return this.Redirect("/Login?eventID=" + eventID);
            }

            this.Approvals = this._approvalRepository.GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.ApprovalID.ToString(),
                    Text = a.Name
                })
                .ToList();
            this.Priorities = this._priorityRepository.GetAll()
                .Select(p => new SelectListItem
                {
                    Value = p.PriorityID.ToString(),
                    Text = p.Name
                })
                .ToList();
            this.Messages = this._messageRepository.GetAllByEventID(eventID)
                .OrderBy(m => m.EnteredOn)
                .ToList();

            this.ProposedSenders = this._messageRepository.GetProposedSenders(eventID).ToList();
            this.ProposedRecipients = this._messageRepository.GetProposedRecipients(eventID).ToList();

            if (messageID.HasValue)
            {
                var currentMessage = this.Messages.FirstOrDefault(m => m.MessageID == messageID.Value);
                if (currentMessage != null)
                {
                    this.EnteredOn = currentMessage.EnteredOn.ToString("yyyy-MM-dd HH:mm");
                    this.Sender = currentMessage.Sender;
                    this.Recipient = currentMessage.Recipient;
                    this.MessageText = currentMessage.MessageText;
                    this.PriorityID = currentMessage.PriorityID;
                    this.ApprovalID = currentMessage.ApprovalID;
                }
            }
            var ev = this._eventRepository.GetAll().FirstOrDefault(e => e.EventID == eventID);
            if (ev != null)
            {
                this.EventName = ev.EventName;
                this.ViewData["Title"] = this.EventName;
            }
            return null;
        }

        public IActionResult OnPost(int eventID, int? messageID = null)
        {
            if (!this.CheckAuthentication())
            {
                return this.Redirect("/Login?eventID=" + eventID);
            }
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
                    MessageText = this.MessageText,
                    PriorityID = this.PriorityID,
                    ApprovalID = this.ApprovalID
                };
                if (messageID.HasValue)
                {
                    message.MessageID = messageID.Value;
                }
                message = this._messageRepository.Save(message);
                return this.Redirect("/Message/" + eventID + "#" + message.MessageAnchor);
            }
            return this.Redirect("/Message/" + eventID);
        }

        public IActionResult OnPostDelete(int eventID, int? messageID)
        {
            if (!this.CheckAuthentication())
            {
                return this.Redirect("/Login?eventID=" + eventID);
            }
            if (messageID.HasValue)
            {
                this._messageRepository.Delete(messageID.Value);
            }
            return this.Redirect("/Message/" + eventID);
        }
    }
}
