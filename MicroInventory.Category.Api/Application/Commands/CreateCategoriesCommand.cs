using MediatR;
using MicroInventory.Category.Api.Domain.Entities;

namespace MicroInventory.Category.Api.Application.Commands
{
    public class CreateCategoriesCommand : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
