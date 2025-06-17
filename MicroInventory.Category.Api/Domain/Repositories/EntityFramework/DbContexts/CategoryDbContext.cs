using MicroInventory.Category.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Category.Api.Domain.Repositories.EntityFramework.DbContexts
{
    public class CategoryDbContext(DbContextOptions<CategoryDbContext> options) : DbContext(options)
    {
        public DbSet<Categories> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoriesEntityTypeConfiguration());
        }
    }
}
