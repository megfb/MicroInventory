using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Assignment.Api.Application.IntegrationEvents.EventHandlers
{
    public class PersonUpdatedEventHandler(AssignmentDbContext context) : IIntegrationEventHandler<PersonUpdatedIntegrationEvent>
    {
        private readonly AssignmentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task Handle(PersonUpdatedIntegrationEvent @event)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == @event.PersonId);
            if (person != null)
            {
                person.Name = @event.Name;
                _context.Persons.Update(person);
                await _context.SaveChangesAsync();
                Console.WriteLine($"PersonUpdatedEventHandler İşlem Başarılı: {@event.PersonId} - {@event.Name} güncellendi.");
            }
        }
    }
}
