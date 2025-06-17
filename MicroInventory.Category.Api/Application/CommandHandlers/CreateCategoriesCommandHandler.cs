using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Domain.Entities;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Category.Api.Application.CommandHandlers
{
    public class CreateCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork,ILogger<CreateCategoriesCommandHandler> logger) : IRequestHandler<CreateCategoriesCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        public async Task<Guid> Handle(CreateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var category = new Categories
            {
                Id = new Guid(),
                CreatedAt = DateTime.UtcNow,
                Name = request.Name,
                Description = request.Description,
            };
            await _categoryRepository.CreateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("New category is created");
            return category.Id;
        }
    }
}
