using MediatR;
using MicroInventory.Category.Api.Application.Dtos;

namespace MicroInventory.Category.Api.Application.Queries
{
    public class GetCategoryByIdQuery:IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
    }
}
