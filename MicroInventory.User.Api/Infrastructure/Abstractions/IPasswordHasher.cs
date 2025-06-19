namespace MicroInventory.User.Api.Infrastructure.Abstractions
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        bool Verify(string password, string hashedPassword);
    }
}
