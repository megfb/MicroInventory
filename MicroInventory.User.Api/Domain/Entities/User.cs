using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.User.Api.Domain.Entities
{
    public class User:Entity<string>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; } = "Admin";
    }
}
