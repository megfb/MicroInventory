using MediatR;
using MicroInventory.Category.Api.Application.Dtos;

namespace MicroInventory.Category.Api.Application.Queries
{
    public class GetCategoriesQuery:IRequest<IEnumerable<CategoryDto>>
    {

    }
}
