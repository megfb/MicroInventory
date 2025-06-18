using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.Commands
{
    public class DeletePersonCommand:IRequest<Result>
    {
        public string Id { get; set; }
    }
}
