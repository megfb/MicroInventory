using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Stock.Api.Application.IntegrationEvents.EventHandlers
{
    public class ProductUpdatedEventHandler(StockDbContext context) : IIntegrationEventHandler<ProductUpdatedIntegrationEvent>
    {
        private readonly StockDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(ProductUpdatedIntegrationEvent @event)
        {
            var product = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == @event.ProductId);
            if (product != null)
            {
                product.ProductName = @event.Name;
                _context.Stocks.Update(product);
                await _context.SaveChangesAsync();
                Console.WriteLine("GÜNCELLEME İŞLEMİ TAMAMLANDI");
            }
        }
    }
}
