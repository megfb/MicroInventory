using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.User.Api.Application.Commands
{
    public class RegisterUserCommand:IRequest<string>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
