using AutoMapper;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Queries.GetSucursal
{
    public class GetSucursalMappingProfile : Profile
    {
        public GetSucursalMappingProfile()
        {
            CreateMap<Domain.Entities.Sucursal, GetSucursalDTO>();
        }
    }
}
