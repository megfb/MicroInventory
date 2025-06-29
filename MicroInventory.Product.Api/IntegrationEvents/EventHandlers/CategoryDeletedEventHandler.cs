using MicroInventory.Product.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Product.Api.IntegrationEvents.EventHandlers
{
    public class CategoryDeletedEventHandler(ProductDbContext context) : IIntegrationEventHandler<CategoryDeletedIntegrationEvent>
    {
        private readonly ProductDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(CategoryDeletedIntegrationEvent @event)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == @event.CategoryId);
            if (category != null)
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
            }
            Console.WriteLine("SİLME İŞLEMİ TAMAMLANDI");
        }
    }
}
