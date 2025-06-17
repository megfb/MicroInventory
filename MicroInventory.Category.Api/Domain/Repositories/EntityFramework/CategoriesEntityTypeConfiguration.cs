using MicroInventory.Category.Api.Domain.Entities;
using MicroInventory.Shared.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroInventory.Category.Api.Domain.Repositories.EntityFramework
{
    public class CategoriesEntityTypeConfiguration : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);
            
        }
    }
}
