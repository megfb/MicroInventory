using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Stock.Api.Domain.Repositories.Abstractions;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Stock.Api.Application.IntegrationEvents.EventHandlers
{
    public class AssignmentDeletedEventHandler(StockDbContext context) : IIntegrationEventHandler<AssignmentDeletedIntegrationEvent>
    {
        private readonly StockDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task Handle(AssignmentDeletedIntegrationEvent @event)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == @event.ProductId);

            stock.StockCount += 1;

            _context.Update(stock);
            await _context.SaveChangesAsync();
            Console.WriteLine($"AssignmentDeletedEventHandler İşlem Başarılı: {@event.ProductId} ataması silindi. Stok güncellendi: {stock.StockCount} adet kaldı.");
        }
    }

}
