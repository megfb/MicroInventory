using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Stock.Api.Application.Commands
{
    public class CreateStockCommand:IRequest<Result>
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int StockCount { get; set; } = 20;

        public int MinThreshold { get; set; } = 5;
    }
}
