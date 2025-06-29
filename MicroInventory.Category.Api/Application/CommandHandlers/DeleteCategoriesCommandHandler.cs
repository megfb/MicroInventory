using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Category.Api.Application.CommandHandlers
{
    public class DeleteCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ILogger<DeleteCategoriesCommandHandler> logger,
        IEventBus eventBus) : IRequestHandler<DeleteCategoriesCommand, Result>
    {
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteCategoriesCommandHandler> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            await _categoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("category is deleted");
            var @event = new CategoryDeletedIntegrationEvent
            {
                CategoryId = request.Id
            };
            await _eventBus.PublishAsync(@event, "category-events-topic");
            return new Result(true, "Category is deleted");
        }
    }
}
