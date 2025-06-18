using MediatR;
using MicroInventory.Person.Api.Application.Dtos;
using MicroInventory.Person.Api.Application.Queries;
using MicroInventory.Person.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.QueryHandlers
{
    public class GetPersonByIdQueryHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork, ILogger<GetPersonByIdQueryHandler> logger) : IRequestHandler<GetPersonByIdQuery, IDataResult<PersonDto>>
    {
        private readonly IPersonRepository _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<GetPersonByIdQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<IDataResult<PersonDto>> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.Id);

            if (person == null)
                throw new KeyNotFoundException($"Person with ID {request.Id} not found.");
            _logger.LogInformation("Person is found");
            return new SuccessDataResult<PersonDto>(new PersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                PhoneNumber = person.PhoneNumber,
                Department = person.Department,
                CreatedAt = person.CreatedAt,
                UpdatedAt = person.UpdatedAt
            });
        }
    }
}
