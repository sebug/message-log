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

        public Event Save(Event e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }
            if (e.EventID > 0)
            {
                this._eventContext.Event.Attach(e);
                this._eventContext.Entry(e).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                this._eventContext.Event.Add(e);
            }
            this._eventContext.SaveChanges();
            return e;
        }
    }
}
