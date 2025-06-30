using MediatR;
using MicroInventory.Product.Api.Application.Commands;
using MicroInventory.Product.Api.Domain.Entities;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Product.Api.Application.CommandHandlers
{
    public class CreateProductsCommandHandler(IProductRepository productRepository, ILogger<CreateProductsCommandHandler> logger, IUnitOfWork unitOfWork,
        IEventBus eventBus) : IRequestHandler<CreateProductsCommand, Result>
    {
        private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<CreateProductsCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        public async Task<Result> Handle(CreateProductsCommand request, CancellationToken cancellationToken)
        {
            var product = new Products
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Name = request.Name,
                Description = request.Description,
                Brand = request.Brand,
                Model = request.Model,
                CategoryId = request.CategoryId,
                //StockCount = request.StockCount
            };

            await _productRepository.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("New product is created with ID: {ProductId}", product.Id);
            await _eventBus.PublishAsync(new ProductAddedIntegrationEvent
            {
                ProductId = product.Id,
                Name = product.Name,
            }, "product-events-topic");
            return new Result(true, "Product created successfully");
        }
    }
}

