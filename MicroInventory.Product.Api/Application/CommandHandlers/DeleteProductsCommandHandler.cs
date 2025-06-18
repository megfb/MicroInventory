using MediatR;
using MicroInventory.Product.Api.Application.Commands;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.CommandHandlers
{
    public class DeleteProductsCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<DeleteProductsCommandHandler> logger) : IRequestHandler<DeleteProductsCommand, Result>
    {
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
            return new Result(true, "Product deleted successfully");
        }
    }
}
