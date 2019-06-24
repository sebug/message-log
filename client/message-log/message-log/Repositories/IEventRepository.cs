using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using message_log.Models;

namespace message_log.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAll();
        Event Save(Event e);
    }
}
