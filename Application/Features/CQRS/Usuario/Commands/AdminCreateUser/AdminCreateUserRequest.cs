using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Domain.Enums;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.AdminCreateUser
{
    public class AdminCreateUserRequest : IRequest<Response>
    {
        public long Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public RolUsuario Rol { get; set; }
    }
}
