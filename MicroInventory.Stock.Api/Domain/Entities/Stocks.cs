using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Stock.Api.Domain.Entities
{
    public class Stocks:Entity<string>
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int StockCount { get; set; } = 20;

        public int MinThreshold { get; set; } = 5;

    }
}
