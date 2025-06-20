using MicroInventory.Shared.Common.Domain;
using MicroInventory.User.Api.Domain.Repositories.Abstractions;
using MicroInventory.User.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.User.Api.Domain.Repositories.EntityFramework
{
    public class UserRepository(UserDbContext context, IUnitOfWork unitOfWork) : IUserRepository
    {
        public async Task<Entities.User> CreateAsync(Entities.User user)
        {
            await context.User.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<Entities.User> GetByEmailAsync(string email)
        {
            return await context.User.FirstOrDefaultAsync(x => x.Email == email);
                
        }

        public async Task<Entities.User> GetByIdAsync(string id)
        {
            return await context.User
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public async Task<Entities.User> GetByUsernameAsync(string username)
        {
            return await context.User
                .FirstOrDefaultAsync(x => x.Username == username);
        }
    }
}
