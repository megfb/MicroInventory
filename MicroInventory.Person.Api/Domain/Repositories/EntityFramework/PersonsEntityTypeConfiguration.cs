using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MicroInventory.Person.Api.Domain.Entities;

namespace MicroInventory.Person.Api.Domain.Repositories.EntityFramework
{
    public class PersonsEntityTypeConfiguration : IEntityTypeConfiguration<Persons>
    {
        public void Configure(EntityTypeBuilder<Persons> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(p => p.Department)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
