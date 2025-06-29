using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Product.Api.Application.Commands;

public class CreateProductsCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int StockCount { get; set; }
    public string CategoryId { get; set; }
}