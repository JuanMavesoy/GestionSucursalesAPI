using GestionSucursalesAPI.Application.DTOs;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser
{
    public class GetUsersRequest : RequestPaginated, IRequest<GetUsersResponse>
    {
        public int Id { get; set; }
    }
}
