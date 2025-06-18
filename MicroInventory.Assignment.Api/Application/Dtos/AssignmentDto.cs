namespace MicroInventory.Assignment.Api.Application.Dtos
{
    public class AssignmentDto
    {
        public string Id { get; set; }
        public string ProductId { get; set; }

        public string PersonId { get; set; }

        public DateTime AssignedAt { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
