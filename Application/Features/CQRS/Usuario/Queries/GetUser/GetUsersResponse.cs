using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Application.Helpers;
using GestionSucursalesAPI.Domain.Enums;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser
{
    public class GetUsersResponse : ResponsePaginated
    {
        public PaginatedList<GetUsersDTO> Data { get; set; }
    }

    public class GetUsersDTO
    {
        public int Id { get; set; }
        public long Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string Username { get; set; }

      //  public RolUsuario Rol { get; set; }
    }
}
