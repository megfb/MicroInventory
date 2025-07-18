﻿using MediatR;
using MicroInventory.Product.Api.Application.Commands;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Product.Api.Application.CommandHandlers
{
    public class UpdateProductsCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<UpdateProductsCommandHandler> logger,
        IEventBus eventBus) : IRequestHandler<UpdateProductsCommand, Result>
    {
        private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<UpdateProductsCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        public async Task<Result> Handle(UpdateProductsCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");
            product.Name = request.Name;
            product.Description = request.Description;
            product.Brand = request.Brand;
            product.Model = request.Model;
            //product.StockCount = request.StockCount;
            product.CategoryId = request.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Product with ID {ProductId} updated successfully", request.Id);
            await _eventBus.PublishAsync(new ProductUpdatedIntegrationEvent
            {
                ProductId = request.Id,
                Name = request.Name,
            },"product-events-topic");
            return new Result(true, "Product updated successfully");
        }
    }
}
