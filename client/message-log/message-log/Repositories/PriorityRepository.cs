using System;
using System.Collections.Generic;
using System.Linq;
using message_log.Models;

namespace message_log.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly PriorityContext _priorityContext;
        public PriorityRepository(PriorityContext priorityContext)
        {
            this._priorityContext = priorityContext;
        }

        public IEnumerable<Priority> GetAll()
        {
            return this._priorityContext.Priority.ToList();
        }
    }
}
