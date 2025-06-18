using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Assignment.Api.Domain.Repositories
{
    public class UnitOfWork(AssignmentDbContext context) : IUnitOfWork
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
