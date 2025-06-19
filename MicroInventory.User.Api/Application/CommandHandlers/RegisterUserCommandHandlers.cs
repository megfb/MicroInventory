using MediatR;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.User.Api.Application.Commands;
using MicroInventory.User.Api.Domain.Repositories.Abstractions;
using MicroInventory.User.Api.Infrastructure.Abstractions;

namespace MicroInventory.User.Api.Application.CommandHandlers
{
    public class RegisterUserCommandHandlers(IUserRepository userRepository, IUnitOfWork unitOfWork,
        ILogger<RegisterUserCommandHandlers> logger, IJwtTokenGenerator jwtTokenGenerator,IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<RegisterUserCommandHandlers> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
            var existingUserByUsername = await _userRepository.GetByUsernameAsync(request.Username);

            if (existingUserByEmail is not null || existingUserByUsername is not null)
                throw new Exception("Email veya kullanıcı adı zaten kullanımda.");


            var passwordHash = _passwordHasher.HashPassword(request.PasswordHash);
            var user = new Domain.Entities.User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Username = request.Username,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
            };
            await _userRepository.CreateAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Kullanıcı kaydı başarılı: {Username}", request.Username);
            var token = _jwtTokenGenerator.GenerateToken(user);
            return token;
        }
    }
}
