﻿using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Domain.Entities;
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Category.Api.Application.CommandHandlers
{
    public class CreateCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ILogger<CreateCategoriesCommandHandler> logger,
        IEventBus eventBus) : IRequestHandler<CreateCategoriesCommand, Result>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<CreateCategoriesCommandHandler> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        public async Task<Result> Handle(CreateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var category = new Categories
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Name = request.Name,
                Description = request.Description,
            };
            await _categoryRepository.CreateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("New category is created");
            var @event = new CategoryCreatedIntegrationEvent()
            {
                CategoryId = category.Id,
                Name = category.Name,

            };
            await _eventBus.PublishAsync(@event, "category-events-topic");
            return new Result(true, "Category created successfully");
        }
    }
}
