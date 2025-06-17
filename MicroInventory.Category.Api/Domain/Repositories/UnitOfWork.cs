using MicroInventory.Category.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Category.Api.Domain.Repositories
{
    public class UnitOfWork(CategoryDbContext context) : IUnitOfWork
    {
        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
