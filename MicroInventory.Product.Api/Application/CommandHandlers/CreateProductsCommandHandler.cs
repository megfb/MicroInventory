using MediatR;
using MicroInventory.Product.Api.Application.Commands;
using MicroInventory.Product.Api.Domain.Entities;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.CommandHandlers
{
    public class CreateProductsCommandHandler(IProductRepository productRepository, ILogger<CreateProductsCommandHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<CreateProductsCommand, Result>
    {
        private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<CreateProductsCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                CategoryId = request.CategoryId
            };

            await _productRepository.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("New product is created with ID: {ProductId}", product.Id);
            return new Result(true, "Product created successfully");
        }
    }
}

