using System;
using message_log.Models;
using Microsoft.EntityFrameworkCore;

namespace message_log.Repositories
{
    public class PriorityContext : DbContext
    {
        public PriorityContext(DbContextOptions<PriorityContext> options) : base(options)
        {
        }

        public DbSet<Priority> Priority { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
