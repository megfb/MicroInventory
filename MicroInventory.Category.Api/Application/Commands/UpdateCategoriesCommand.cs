using MediatR;

namespace MicroInventory.Category.Api.Application.Commands
{
    public class UpdateCategoriesCommand:IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
