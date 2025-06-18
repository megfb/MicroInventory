using MicroInventory.Product.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroInventory.Product.Api.Domain.Repositories.EntityFramework
{
    public class ProductsEntityTypeConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.Brand).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Model).IsRequired().HasMaxLength(100);
        }
    }
}
