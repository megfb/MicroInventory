using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Stock.Api.Domain.Repositories.Abstractions;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Stock.Api.Application.IntegrationEvents.EventHandlers
{
    public class AssignmentAddedEventHandler(StockDbContext context) : IIntegrationEventHandler<AssignmentAddedIntegrationEvent>
    {
        private readonly StockDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task Handle(AssignmentAddedIntegrationEvent @event)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == @event.ProductId);

            stock.StockCount -= 1;

            if (stock.StockCount <= stock.MinThreshold)
            {
                stock.StockCount += 10;
            }

            context.Stocks.Update(stock);
            await context.SaveChangesAsync();
            Console.WriteLine($"AssignmentAddedEventHandler İşlem Başarılı: {@event.ProductId} ataması yapıldı. Stok güncellendi: {stock.StockCount} adet kaldı.");
        }
    }
}
