using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Category.Api.IntegrationEvents.EventHandlers
{
    public class ProductAddedEventHandler: IIntegrationEventHandler<ProductAddedIntegrationEvent>
    {
        public Task Handle(ProductAddedIntegrationEvent @event)
        {
            // Bu noktada @event.CategoryId, @event.ProductName vb. üzerinden işlem yapabilirsin.
            Console.WriteLine($"[CategoryService] Yeni ürün eklendi: {@event.ProductName} (Kategori: {@event.CategoryId})");

            // Örneğin: Kategoriye bağlı ürün sayısını güncellemek gibi işlemler burada yapılabilir.
            return Task.CompletedTask;
        }
    }
}
