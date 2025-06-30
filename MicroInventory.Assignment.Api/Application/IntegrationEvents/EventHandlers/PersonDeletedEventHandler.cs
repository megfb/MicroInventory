using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Assignment.Api.Application.IntegrationEvents.EventHandlers
{
    public class PersonDeletedEventHandler(AssignmentDbContext context) : IIntegrationEventHandler<PersonDeletedIntegrationEvent>
    {
        private readonly AssignmentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(PersonDeletedIntegrationEvent @event)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == @event.PersonId);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
                Console.WriteLine($"PersonDeletedEventHandler İşlem Başarılı: {@event.PersonId} silindi.");
            }
        }
    }
}
