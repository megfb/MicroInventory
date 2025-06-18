using MediatR;
using MicroInventory.Category.Api.Application.Dtos;
using MicroInventory.Category.Api.Application.Queries;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Category.Api.Application.QueryHandlers
{
    public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, ILogger<GetCategoryByIdQueryHandler> logger) : IRequestHandler<GetCategoryByIdQuery, IDataResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly ILogger<GetCategoryByIdQueryHandler> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<IDataResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category is null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");

            logger.LogInformation("Category is gotten");

            return new SuccessDataResult<CategoryDto>(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
            });
        }
    }
}
