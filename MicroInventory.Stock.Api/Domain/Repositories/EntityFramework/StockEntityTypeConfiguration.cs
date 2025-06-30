using MicroInventory.Stock.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroInventory.Stock.Api.Domain.Repositories.EntityFramework
{
    public class StockEntityTypeConfiguration : IEntityTypeConfiguration<Stocks>
    {
        public void Configure(EntityTypeBuilder<Stocks> builder)
        {

            builder.HasKey(s => s.Id);
            builder.Property(s => s.ProductName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.StockCount).IsRequired();
        }
    }
}
