using System;
using message_log.Models;
using Microsoft.EntityFrameworkCore;

namespace message_log.Repositories
{
    public class ApprovalContext : DbContext
    {
        public ApprovalContext(DbContextOptions<ApprovalContext> options) : base(options)
        {
        }

        public DbSet<Approval> Approval { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
