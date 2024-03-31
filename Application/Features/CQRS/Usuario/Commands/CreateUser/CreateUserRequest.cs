using GestionSucursalesAPI.Application.DTOs;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.CreateUser
{
    public class CreateUserRequest : IRequest<Response>
    {
        public long Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
