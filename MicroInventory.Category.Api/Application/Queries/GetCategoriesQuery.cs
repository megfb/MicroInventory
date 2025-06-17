using MediatR;
using MicroInventory.Category.Api.Application.Dtos;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Category.Api.Application.Queries
{
    public class GetCategoriesQuery : IRequest<IDataResult<IEnumerable<CategoryDto>>>
    {

    }
}
