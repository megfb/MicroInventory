using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Category.Api.Application.CommandHandlers
{
    public class DeleteCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork,ILogger<DeleteCategoriesCommandHandler> logger) : IRequestHandler<DeleteCategoriesCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        public async Task<Guid> Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            await _categoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("category is deleted");
            return category.Id;
        }
    }
}
