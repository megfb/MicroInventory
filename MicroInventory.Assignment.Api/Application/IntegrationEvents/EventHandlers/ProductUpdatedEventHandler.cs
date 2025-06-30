using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Assignment.Api.Application.IntegrationEvents.EventHandlers
{
    public class ProductUpdatedEventHandler(AssignmentDbContext context) : IIntegrationEventHandler<ProductUpdatedIntegrationEvent>
    {
        private readonly AssignmentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(ProductUpdatedIntegrationEvent @event)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == @event.ProductId);
            if(product!= null)
            {
                product.Name = @event.Name;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                Console.WriteLine("GÜNCELLEME İŞLEMİ TAMAMLANDI");
            }
        }
    }
}
