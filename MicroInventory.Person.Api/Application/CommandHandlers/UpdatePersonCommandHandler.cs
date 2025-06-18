using MediatR;
using MicroInventory.Person.Api.Application.Commands;
using MicroInventory.Person.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.CommandHandlers
{
    public class UpdatePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork, ILogger<UpdatePersonCommandHandler> logger) : IRequestHandler<UpdatePersonCommand, Result>
    {
        private readonly IPersonRepository _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<UpdatePersonCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.Id);
            if (person == null)
                throw new KeyNotFoundException("Person not found");
            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            person.Email = request.Email;
            person.PhoneNumber = request.PhoneNumber;
            person.Department = request.Department;
            person.UpdatedAt = DateTime.UtcNow;
            await _personRepository.UpdateAsync(person);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Person with ID {Id} updated successfully", request.Id);
            return new Result(true, "Person has successfully updated");
        }
    }
}
