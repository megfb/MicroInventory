using MicroInventory.Product.Api.Domain.Entities;

namespace MicroInventory.Product.Api.Domain.Repositories.Abstractions
{
    public interface IProductRepository
    {
        Task<Products> CreateAsync(Products products);
        Task UpdateAsync(Products products);
        Task DeleteAsync(Products products);
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products> GetByIdAsync(string id);

    }
}
