using MicroInventory.Assignment.Api.Domain.Entities;
using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Assignment.Api.Application.IntegrationEvents.EventHandlers
{
    public class PersonAddedEventHandler(AssignmentDbContext context) : IIntegrationEventHandler<PersonAddedIntegrationEvent>
    {
        private readonly AssignmentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(PersonAddedIntegrationEvent @event)
        {
            var exist = _context.Persons.Any(p => p.Id == @event.PersonId);
            if (!exist)
            {
                await _context.Persons.AddAsync(new PersonReadModel
                {
                    Id = @event.PersonId,
                    Name = @event.Name,
                });
                await _context.SaveChangesAsync();
                Console.WriteLine($"PersonAddedEventHandler İşlem Başarılı: {@event.PersonId} - {@event.Name} eklendi.");
            }
        }
    }
}
