using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.Commands
{
    public class DeleteProductsCommand : IRequest<Result>
    {
        public string Id { get; set; }
    }
}
