using GestionSucursalesAPI.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionSucursalesAPI.Infraestructure.Services.JWT
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario user)
        {
            if (string.IsNullOrEmpty(_configuration["Jwt:Key"]) || string.IsNullOrEmpty(_configuration["Jwt:Issuer"]))
            {
                // Manejar la falta de configuración adecuadamente
                return null;
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = GenerateClaims(user);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(6), // Expiración en 6 horas
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IEnumerable<Claim> GenerateClaims(Usuario user)
        {
            yield return new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            yield return new Claim(ClaimTypes.Name, $"{user.Nombres} {user.Apellidos}");
            yield return new Claim(ClaimTypes.UserData, user.Username);
        }
    }
}
