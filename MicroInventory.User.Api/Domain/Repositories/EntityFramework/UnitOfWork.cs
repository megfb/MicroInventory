using MicroInventory.Shared.Common.Domain;
using MicroInventory.User.Api.Domain.Repositories.EntityFramework.DbContexts;

namespace MicroInventory.User.Api.Domain.Repositories.EntityFramework
{
    public class UnitOfWork(UserDbContext context):IUnitOfWork
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
