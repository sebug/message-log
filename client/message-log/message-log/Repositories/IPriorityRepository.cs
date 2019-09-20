using System;
using System.Collections.Generic;
using message_log.Models;

namespace message_log.Repositories
{
    public interface IPriorityRepository
    {
        IEnumerable<Priority> GetAll();
    }
}
