using MicroInventory.User.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.User.Api.Domain.Repositories.EntityFramework.DbContexts
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<Entities.User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}
