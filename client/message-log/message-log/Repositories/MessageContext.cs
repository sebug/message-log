using System;
using message_log.Models;
using Microsoft.EntityFrameworkCore;

namespace message_log.Repositories
{
    public class MessageContext : DbContext
    {
        public DbSet<Message> Message { get; set; }

        public MessageContext(DbContextOptions<MessageContext> options) : base(options)
        {

        }
    }
}
