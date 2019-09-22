using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using message_log.Models;
using message_log.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace message_log.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string EventName { get; set; }

        public List<Event> Events { get; set; }

        private IEventRepository _eventRepository;
        private readonly IAuthenticationService _authenticationService;
        public IndexModel(IEventRepository eventRepository,
            IAuthenticationService authenticationService)
        {
            this._eventRepository = eventRepository;
            this._authenticationService = authenticationService;
        }

        public void OnGet()
        {
            this.Events = this._eventRepository.GetAll().ToList();
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

        public IActionResult OnPost()
        {
            if (!String.IsNullOrEmpty(this.EventName))
            {
                if (!this.CheckAuthentication())
                {
                    return this.Redirect("Login");
                }
                var ev = new Event
                {
                    EventName = this.EventName
                };
                ev = this._eventRepository.Save(ev);
                return this.Redirect(ev.MessageLink);
            }
            return this.Redirect("/");
        }
    }
}
