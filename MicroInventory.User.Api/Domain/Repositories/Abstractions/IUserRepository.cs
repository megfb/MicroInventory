using MicroInventory.User.Api.Domain.Entities;

namespace MicroInventory.User.Api.Domain.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<User.Api.Domain.Entities.User> GetByIdAsync(string id);
        Task<User.Api.Domain.Entities.User> GetByUsernameAsync(string username);
        Task<User.Api.Domain.Entities.User> GetByEmailAsync(string email);
        Task<User.Api.Domain.Entities.User> CreateAsync(User.Api.Domain.Entities.User user);
    }
}
