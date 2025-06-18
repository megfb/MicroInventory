namespace MicroInventory.Category.Api.Application.Dtos
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
