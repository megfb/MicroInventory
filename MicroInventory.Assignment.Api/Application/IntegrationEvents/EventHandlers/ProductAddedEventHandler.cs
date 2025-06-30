using MicroInventory.Assignment.Api.Domain.Entities;
using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Assignment.Api.Application.IntegrationEvents.EventHandlers
{
    public class ProductAddedEventHandler(AssignmentDbContext context) : IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly AssignmentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(ProductAddedIntegrationEvent @event)
        {
            var exists = _context.Products.Any(x => x.Id == @event.ProductId);
            if (!exists)
            {
                await _context.AddAsync(new ProductReadModel
                {
                    Id = @event.ProductId,
                    Name = @event.Name
                });
                await _context.SaveChangesAsync();
                Console.WriteLine($"ProductAddedEventHandler İşlem Başarılı: {@event.ProductId} - {@event.Name} eklendi.");
            }
        }
    }
}
