using MediatR;
using MicroInventory.Person.Api.Application.Commands;
using MicroInventory.Person.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Person.Api.Application.CommandHandlers
{
    public class DeletePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork, ILogger<DeletePersonCommandHandler> logger,IEventBus eventBus) : IRequestHandler<DeletePersonCommand, Result>
    {
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        private readonly IPersonRepository _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeletePersonCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.Id);
            if (person == null)
                throw new KeyNotFoundException("Person not found");
            await _personRepository.DeleteAsync(person);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Person with ID {Id} deleted successfully", request.Id);
            await _eventBus.PublishAsync(new PersonDeletedIntegrationEvent { PersonId = person.Id }, "person-events-topic");
            return new Result(true, "Person has successfully deleted");
        }
    }
}
