using System;
using message_log.Models;
using Microsoft.EntityFrameworkCore;

namespace message_log.Repositories
{
    public class EventContext : DbContext
    {
        public DbSet<Event> Event { get; set; }

        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
