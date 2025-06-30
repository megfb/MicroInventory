using MicroInventory.Stock.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts
{
    public class StockDbContext(DbContextOptions<StockDbContext> options) : DbContext(options)
    {
        public DbSet<Stocks> Stocks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);
        }
    }
}
