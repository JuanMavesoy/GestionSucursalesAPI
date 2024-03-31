using GestionSucursalesAPI.Application.DTOs;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.DeleteUser
{
    public class DeleteUserRequest : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
