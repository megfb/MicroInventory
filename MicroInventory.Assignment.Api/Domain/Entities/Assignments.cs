using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Assignment.Api.Domain.Entities
{
    public class Assignments : Entity<string>
    {
        public string ProductId { get; set; }

        public string PersonId { get; set; }

        public DateTime AssignedAt { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public string? Notes { get; set; }  
    }
}
