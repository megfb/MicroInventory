using MediatR;
using MicroInventory.Person.Api.Application.Commands;
using MicroInventory.Person.Api.Domain.Entities;
using MicroInventory.Person.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.CommandHandlers
{
    public class CreatePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork, ILogger<CreatePersonCommandHandler> logger) : IRequestHandler<CreatePersonCommand, Result>
    {
        private readonly IPersonRepository _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<CreatePersonCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = new Persons
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Department = request.Department,
                CreatedAt = DateTime.UtcNow,
            };
            await _personRepository.CreateAsync(person);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Person created successfully with ID: {Id}", person.Id);
            return new Result(true, "Person created successfully");

        }
    }
}
