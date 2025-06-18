using MicroInventory.Assignment.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork
{
    public class AssignmentsEntityTypeConfiguration : IEntityTypeConfiguration<Assignments>
    {
        public void Configure(EntityTypeBuilder<Assignments> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Notes).HasMaxLength(200).IsRequired(false);
            builder.Property(a => a.ReturnedAt).IsRequired(false);
        }
    }
}
