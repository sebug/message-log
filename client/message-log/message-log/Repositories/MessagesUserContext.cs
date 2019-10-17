using System;
using message_log.Models;
using Microsoft.EntityFrameworkCore;

namespace message_log.Repositories
{
    public class MessagesUserContext : DbContext
    {
        public DbSet<MessagesUser> MessagesUser { get; set; }

        public MessagesUserContext(DbContextOptions<MessagesUserContext> options) : base(options)
        {

        }
    }
}
