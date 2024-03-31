using GestionSucursalesAPI.Application.DTOs;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.UpdateUser
{
    public class UpdateUserRequest : IRequest<Response>
    {
        public int Id { get; set; }
        public long Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
