using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Event> Events { get; set; }

        private IEventRepository _eventRepository;
        public IndexModel(IEventRepository eventRepository)
        {
            this._eventRepository = eventRepository;
        }

        public void OnGet()
        {
            this.Events = this._eventRepository.GetAll().ToList();
        }
    }
}
