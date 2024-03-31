using AutoMapper;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser
{
    public class GetUsersMappingProfile : Profile
    {
        public GetUsersMappingProfile()
        {
            CreateMap<Domain.Entities.Usuario, GetUsersDTO>();
        }
    }
}
