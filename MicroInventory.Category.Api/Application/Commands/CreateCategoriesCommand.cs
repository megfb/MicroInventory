using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Category.Api.Application.Commands
{
    public class CreateCategoriesCommand : IRequest<Result>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
