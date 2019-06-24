using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using message_log.Models;
using System.Linq;

namespace message_log.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventContext _eventContext;

        public EventRepository(EventContext eventContext)
        {
            this._eventContext = eventContext;
        }

        public IEnumerable<Event> GetAll()
        {
            return this._eventContext.Event.ToList();
        }
    }
}
