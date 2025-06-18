using MediatR;
using MicroInventory.Category.Api.Application.Dtos;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Category.Api.Application.Queries
{
    public class GetCategoryByIdQuery : IRequest<IDataResult<CategoryDto>>
    {
        public string Id { get; set; }
    }
}
