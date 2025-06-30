using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Assignment.Api.Application.IntegrationEvents.EventHandlers
{
    public class ProductDeletedEventHandler(AssignmentDbContext context) : IIntegrationEventHandler<ProductDeletedIntegrationEvent>
    {
        private readonly AssignmentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(ProductDeletedIntegrationEvent @event)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == @event.ProductId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Product with ID {@event.ProductId} has been deleted from the database.");
            }
        }
    }
}
