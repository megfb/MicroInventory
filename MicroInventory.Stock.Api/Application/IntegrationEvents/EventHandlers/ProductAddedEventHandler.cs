using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Stock.Api.Domain.Entities;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Stock.Api.Application.IntegrationEvents.EventHandlers
{
    public class ProductAddedEventHandler(StockDbContext context) : IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        private readonly StockDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task Handle(ProductAddedIntegrationEvent @event)
    {
        var exists = _context.Stocks.Any(x => x.Id == @event.ProductId);
        if (!exists)
        {
                await _context.AddAsync(new Stocks
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = @event.ProductId,
                    ProductName = @event.Name,
                    CreatedAt = DateTime.UtcNow,
                });
            await _context.SaveChangesAsync();
            Console.WriteLine($"ProductAddedEventHandler İşlem Başarılı: {@event.ProductId} - {@event.Name} eklendi.");
        }
    }
}
}
