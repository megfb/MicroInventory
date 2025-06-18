using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.Commands
{
    public class DropAssignmentCommand : IRequest<Result>
    {
        public string Id { get; set; }
    }
}
