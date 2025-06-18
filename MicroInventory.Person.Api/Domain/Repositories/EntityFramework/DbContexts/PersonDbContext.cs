using MicroInventory.Person.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Person.Api.Domain.Repositories.EntityFramework.DbContexts
{
    public class PersonDbContext(DbContextOptions<PersonDbContext> options) : DbContext(options)
    {
        public DbSet<Persons> Persons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonsEntityTypeConfiguration());
        }
    }
}
