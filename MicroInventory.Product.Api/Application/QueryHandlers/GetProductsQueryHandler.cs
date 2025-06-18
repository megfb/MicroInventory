using MediatR;
using MicroInventory.Product.Api.Application.Dtos;
using MicroInventory.Product.Api.Application.Queries;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.QueryHandlers
{
    public class GetProductsQueryHandler(IProductRepository productRepository, ILogger<GetProductsQueryHandler> logger) : IRequestHandler<GetProductsQuery, IDataResult<IEnumerable<ProductDto>>>
    {
        private readonly IProductRepository productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly ILogger<GetProductsQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<IDataResult<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetAllAsync();
            _logger.LogInformation("All products is gotten");

            return new SuccessDataResult<IEnumerable<ProductDto>>(products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Brand = p.Brand,
                CategoryId = p.CategoryId,
                Model = p.Model,
                UpdatedAt = p.UpdatedAt,
                CreatedAt = p.CreatedAt
            }));

            throw new NotImplementedException();
        }
    }
}
