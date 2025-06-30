using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Stock.Api.Application.IntegrationEvents.EventHandlers
{
    public class ProductDeletedEventHandler(StockDbContext context) : IIntegrationEventHandler<ProductDeletedIntegrationEvent>
    {
        private readonly StockDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(ProductDeletedIntegrationEvent @event)
        {
            var product = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == @event.ProductId);
            if (product != null)
            {
                _context.Stocks.Remove(product);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Product with ID {@event.ProductId} has been deleted from the database.");
            }
        }
    }
}
