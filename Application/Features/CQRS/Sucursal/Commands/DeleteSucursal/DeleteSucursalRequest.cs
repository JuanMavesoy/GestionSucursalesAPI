using GestionSucursalesAPI.Application.DTOs;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.DeleteSucursal
{
    public class DeleteSucursalRequest : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
