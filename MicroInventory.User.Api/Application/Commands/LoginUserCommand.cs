using MediatR;

namespace MicroInventory.User.Api.Application.Commands
{
    public class LoginUserCommand:IRequest<string>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
