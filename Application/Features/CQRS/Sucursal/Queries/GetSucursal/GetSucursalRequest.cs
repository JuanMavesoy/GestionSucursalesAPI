using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Queries.GetSucursal
{
    public class GetSucursalRequest : RequestPaginated, IRequest<GetSucursalResponse>
    {
        public int Id { get; set; }
    }
}
