using GestionSucursalesAPI.Domain.Entities;

namespace GestionSucursalesAPI.Infraestructure.Services.JWT
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Usuario user);
    }
}
