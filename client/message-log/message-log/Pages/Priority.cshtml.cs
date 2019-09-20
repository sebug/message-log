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
    public class PriorityModel : PageModel
    {
        private readonly IPriorityRepository _priorityRepository;

        public PriorityModel(IPriorityRepository priorityRepository)
        {
            this._priorityRepository = priorityRepository;
        }

        public List<Priority> Priorities { get; set; }

        public void OnGet()
        {
            this.Priorities = this._priorityRepository.GetAll().ToList();
        }
    }
}
