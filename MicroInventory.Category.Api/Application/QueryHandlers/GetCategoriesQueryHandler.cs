using MediatR;
using MicroInventory.Category.Api.Application.Dtos;
using MicroInventory.Category.Api.Application.Queries;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Category.Api.Application.QueryHandlers
{
    public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository, ILogger<GetCategoriesQueryHandler> logger) : IRequestHandler<GetCategoriesQuery, IDataResult<IEnumerable<CategoryDto>>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        public async Task<IDataResult<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();

            logger.LogInformation("All categories is gotten");

            return new SuccessDataResult<IEnumerable<CategoryDto>>(categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            }));
        }
    }
}
