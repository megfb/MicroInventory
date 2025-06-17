using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroInventory.Shared.Common.Domain
{
    public interface IUnitOfWork:IDisposable
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync( CancellationToken cancellationToken = default);

    }
}
