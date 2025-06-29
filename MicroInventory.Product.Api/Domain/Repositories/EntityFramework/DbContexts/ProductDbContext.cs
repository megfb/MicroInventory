using MicroInventory.Product.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Product.Api.Domain.Repositories.EntityFramework.DbContexts
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<CategoryReadModel> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductsEntityTypeConfiguration());
        }
    }
}
