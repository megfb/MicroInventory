using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Product.Api.Domain.Entities
{
    public class Products : Entity<string>
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string Brand { get; set; } = default!;

        public string Model { get; set; } = default!;
        public string CategoryId { get; set; } = default!;
    }
}
