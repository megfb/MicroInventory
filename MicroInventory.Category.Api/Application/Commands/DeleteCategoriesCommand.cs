using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Category.Api.Application.Commands
{
    public class DeleteCategoriesCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

    }
}
