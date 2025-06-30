using MicroInventory.Product.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Product.Api.IntegrationEvents.EventHandlers
{
    public class CategoryUpdatedEventHandler(ProductDbContext productDbContext) : IIntegrationEventHandler<CategoryUpdatedIntegrationEvent>
    {
        private readonly ProductDbContext _productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
        public async Task Handle(CategoryUpdatedIntegrationEvent @event)
        {

            var category = _productDbContext.Categories.FirstOrDefault(x => x.Id == @event.CategoryId);
            if (category != null)
            {

                category.Name = @event.Name;
                _productDbContext.Categories.Update(category);
                await _productDbContext.SaveChangesAsync();
            }
            Console.WriteLine("GÜNCELLEME İŞLEMİ TAMAMLANDI");
        }
    }
}
