using MicroInventory.Product.Api.Domain.Entities;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Product.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Product.Api.Domain.Repositories.EntityFramework
{
    public class ProductRepository(ProductDbContext context,IUnitOfWork unitOfWork) : IProductRepository
    {
        public async Task<Products> CreateAsync(Products products)
        {
            await context.Products.AddAsync(products);
            await unitOfWork.SaveChangesAsync();
            return products;

        }

        public async Task DeleteAsync(Products products)
        {
            context.Products.Remove(products);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await context.Products.OrderByDescending(x => x.Name).ToListAsync();
        }

        public async Task<Products> GetByIdAsync(string id)
        {
            return await context.Products
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        public async Task UpdateAsync(Products products)
        {
            context.Products.Update(products);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
