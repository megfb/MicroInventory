using MediatR;
using MicroInventory.Person.Api.Application.Dtos;
using MicroInventory.Person.Api.Application.Queries;
using MicroInventory.Person.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.QueryHandlers
{
    public class GetPersonsQueryHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork, ILogger<GetPersonsQueryHandler> logger) : IRequestHandler<GetPersonsQuery, IDataResult<IEnumerable<PersonDto>>>
    {
        private readonly IPersonRepository _personRespository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<GetPersonsQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<IDataResult<IEnumerable<PersonDto>>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            var persons = await _personRespository.GetAllAsync();
            _logger.LogInformation("All persons is gotten");

            return new SuccessDataResult<IEnumerable<PersonDto>>(persons.Select(p => new PersonDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                Department = p.Department,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }));
        }
    }
}
