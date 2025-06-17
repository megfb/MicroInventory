using MediatR;

namespace MicroInventory.Category.Api.Application.Commands
{
    public class DeleteCategoriesCommand:IRequest<Guid>
    {
        public Guid Id { get; set; }

    }
}
