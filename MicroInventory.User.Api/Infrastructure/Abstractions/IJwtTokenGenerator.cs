using MicroInventory.User.Api.Domain.Entities;

namespace MicroInventory.User.Api.Infrastructure.Abstractions
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User.Api.Domain.Entities.User user);
    }
}
