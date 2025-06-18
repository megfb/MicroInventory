using MediatR;
using MicroInventory.Product.Api.Application.Dtos;
using MicroInventory.Product.Api.Application.Queries;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.QueryHandlers
{
    public class GetProductByIdQueryHandler(IProductRepository productRepository, ILogger<GetProductByIdQueryHandler> logger): IRequestHandler<GetProductByIdQuery, IDataResult<ProductDto>>
    {
        private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly ILogger<GetProductByIdQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<IDataResult<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            _logger.LogInformation("Product with ID {Id} retrieved successfully", request.Id);
            return new SuccessDataResult<ProductDto>(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Model = product.Model,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            });
        }
    }
}
