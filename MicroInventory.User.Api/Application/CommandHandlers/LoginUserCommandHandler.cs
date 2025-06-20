using MediatR;
using MicroInventory.User.Api.Application.Commands;
using MicroInventory.User.Api.Domain.Repositories.Abstractions;
using MicroInventory.User.Api.Infrastructure.Abstractions;

namespace MicroInventory.User.Api.Application.CommandHandlers
{
    public class LoginUserCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher, ILogger<LoginUserCommandHandler> logger) : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private readonly ILogger<LoginUserCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı.");
            bool isPasswordValid = _passwordHasher.Verify(request.PasswordHash, user.PasswordHash);

            if (!isPasswordValid) 
                throw new Exception("Geçersiz şifre.");
            var token = _jwtTokenGenerator.GenerateToken(user);
            _logger.LogInformation("Kullanıcı giriş yaptı: {Email}", request.Email);
            return token;
        }
    }
}
