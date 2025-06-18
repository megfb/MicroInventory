using MediatR;
using MicroInventory.Person.Api.Application.Dtos;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.Queries
{
    public class GetPersonByIdQuery:IRequest<IDataResult<PersonDto>>
    {
        public string Id { get; set; }
    }
}
