using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.Commands
{
    public class UpdateAssignmentCommand:IRequest<Result>
    {
        public string Id { get; set; }
        public string ProductId { get; set; }

        public string PersonId { get; set; }
        public string? Notes { get; set; }
    }
}
