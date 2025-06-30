using MediatR;
using MicroInventory.Product.Api.Application.Commands;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Product.Api.Application.CommandHandlers
{
    public class DeleteProductsCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<DeleteProductsCommandHandler> logger, IEventBus eventBus) : IRequestHandler<DeleteProductsCommand, Result>
    {
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteProductsCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(DeleteProductsCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                throw new KeyNotFoundException("Product with ID {ProductId} not found");

            await _productRepository.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Product with ID {ProductId} deleted successfully", request.Id);
            await _eventBus.PublishAsync(new ProductDeletedIntegrationEvent
            {
                ProductId = request.Id,
            }, "product-events-topic");
            return new Result(true, "Product deleted successfully");
        }
    }
}
