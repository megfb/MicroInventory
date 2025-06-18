using MicroInventory.Assignment.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts
{
    public class AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : DbContext(options)
    {
        public DbSet<Assignments> Assignments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AssignmentsEntityTypeConfiguration());
        }
    }
}
