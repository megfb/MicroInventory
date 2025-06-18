using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Product.Api.Domain.Entities
{
    public class Products:Entity<string>
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Brand { get; set; } = null!;

        public string Model { get; set; } = null!;

        public string CategoryId { get; set; }
    }
}
