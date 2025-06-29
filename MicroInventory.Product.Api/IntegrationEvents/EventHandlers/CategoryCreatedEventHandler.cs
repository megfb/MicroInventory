using MicroInventory.Product.Api.Domain.Entities;
using MicroInventory.Product.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Product.Api.IntegrationEvents.EventHandlers
{
    public class CategoryCreatedEventHandler(ProductDbContext context) : IIntegrationEventHandler<CategoryCreatedIntegrationEvent>
    {
        private readonly ProductDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(CategoryCreatedIntegrationEvent @event)
        {
            Console.WriteLine("İŞLEM BAŞARILI-1");
            var exists = await _context.Categories.AnyAsync(x => x.Id == @event.CategoryId);
            if (!exists)
            {
                Console.WriteLine("İŞLEM BAŞARILI-2 - if koşulu çalıştı");
                _context.Categories.Add(new CategoryReadModel
                {
                    Id = @event.CategoryId,
                    Name = @event.Name,
                });

                await _context.SaveChangesAsync();
                Console.WriteLine($"İŞLEM BAŞARILI - 3 if koşulu bitti");
            }
        }
    }
}
