using MicroInventory.Category.Api.Domain.Entities;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Category.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Category.Api.Domain.Repositories.EntityFramework
{
    public class CategoryRepository(CategoryDbContext context, IUnitOfWork unitOfWork) : ICategoryRepository
    {
        public async Task<Categories> CreateAsync(Categories categories)
        {
            await context.Categories.AddAsync(categories);
            await unitOfWork.SaveChangesAsync();
            return categories;

        }

        public async Task DeleteAsync(Categories categories)
        {
            context.Categories.Remove(categories);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return await context.Categories.OrderByDescending(x => x.Name).ToListAsync();
        }

        public async Task<Categories> GetByIdAsync(Guid id)
        {
            return await context.Categories
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        public async Task UpdateAsync(Categories categories)
        {
            context.Categories.Update(categories);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
