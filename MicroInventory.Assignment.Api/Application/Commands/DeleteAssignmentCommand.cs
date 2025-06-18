using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.Commands
{
    public class DeleteAssignmentCommand:IRequest<Result>
    {
        public string Id { get; set; }
    }
}
