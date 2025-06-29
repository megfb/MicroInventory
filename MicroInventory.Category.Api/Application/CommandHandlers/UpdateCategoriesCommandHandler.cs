using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Category.Api.Application.CommandHandlers
{
    public class UpdateCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ILogger<UpdateCategoriesCommandHandler> logger,
        IEventBus eventBus) : IRequestHandler<UpdateCategoriesCommand, Result>
    {
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<UpdateCategoriesCommandHandler> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(UpdateCategoriesCommand request, CancellationToken cancellationToken)
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
            await _eventBus.PublishAsync(new CategoryUpdatedIntegrationEvent
            {
                CategoryId = category.Id,
                Name = category.Name
            }, "category-events-topic");
            return new Result(true, "Category is updated");
        }
    }
}
