using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Category.Api.Application.CommandHandlers
{
    public class UpdateCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ILogger<UpdateCategoriesCommandHandler> logger) : IRequestHandler<UpdateCategoriesCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        public async Task<Guid> Handle(UpdateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");

            category.Name = request.Name;
            category.Description = request.Description;
            category.UpdatedAt = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Category is updated");
            return category.Id;
        }
    }
}
