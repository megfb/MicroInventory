using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.Commands
{
    public class UpdateProductsCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int StockCount { get; set; }
        public string CategoryId { get; set; }
    }
}
