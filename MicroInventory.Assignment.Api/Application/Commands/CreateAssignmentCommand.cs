using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.Commands
{
    public class CreateAssignmentCommand:IRequest<Result>
    {
        public string ProductId { get; set; }

        public string PersonId { get; set; }

        public DateTime AssignedAt { get; set; }
        public string? Notes { get; set; }
    }
}
