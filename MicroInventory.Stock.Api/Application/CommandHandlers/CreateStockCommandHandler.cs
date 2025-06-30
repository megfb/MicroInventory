using MediatR;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Stock.Api.Application.Commands;
using MicroInventory.Stock.Api.Domain.Entities;
using MicroInventory.Stock.Api.Domain.Repositories.Abstractions;

namespace MicroInventory.Stock.Api.Application.CommandHandlers
{
    public class CreateStockCommandHandler(IStockRepository stockRepository, IUnitOfWork unitOfWork, ILogger<CreateStockCommandHandler> logger,
        IEventBus eventBus) : IRequestHandler<CreateStockCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IStockRepository _stockRepository = stockRepository ?? throw new ArgumentNullException(nameof(stockRepository));
        private readonly ILogger<CreateStockCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        public async Task<Result> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = new Stocks
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ProductId = request.ProductId,
                ProductName = request.ProductName,
            };

            await _stockRepository.CreateAsync(stock);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("New stock is created");


            throw new NotImplementedException();
        }
    }
}
