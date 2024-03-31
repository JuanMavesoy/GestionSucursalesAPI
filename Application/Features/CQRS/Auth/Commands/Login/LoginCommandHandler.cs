using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestionSucursalesAPI.Application.Features.CQRS.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginRequest, string>
    {
        private readonly SucursalesDbContext _db;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public LoginCommandHandler(SucursalesDbContext db, IJwtTokenGenerator tokenGenerator)
        {
            _db = db;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var existUser = await _db.Users.FirstOrDefaultAsync(us => us.Username == request.userName);

            if (existUser == null || !VerifyPassword(existUser.Password, request.password))
            {
                return null;
            }

            var token = _tokenGenerator.GenerateToken(existUser);
            return token;
        }

        private static bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
