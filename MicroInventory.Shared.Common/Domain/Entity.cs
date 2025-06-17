namespace MicroInventory.Shared.Common.Domain
{
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
