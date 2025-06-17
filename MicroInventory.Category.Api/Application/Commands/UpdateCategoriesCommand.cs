using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Category.Api.Application.Commands
{
    public class UpdateCategoriesCommand:IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
