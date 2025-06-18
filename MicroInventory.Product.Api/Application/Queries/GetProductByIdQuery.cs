using MediatR;
using MicroInventory.Product.Api.Application.Dtos;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.Queries
{
    public class GetProductByIdQuery:IRequest<IDataResult<ProductDto>>
    {
        public string Id { get; set; }
    }
}
