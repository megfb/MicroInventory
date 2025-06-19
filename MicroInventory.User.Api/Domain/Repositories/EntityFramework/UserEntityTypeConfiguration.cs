using MicroInventory.User.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroInventory.User.Api.Domain.Repositories.EntityFramework
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User.Api.Domain.Entities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
