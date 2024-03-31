using AutoMapper;
using AutoMapper.QueryableExtensions;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser;
using GestionSucursalesAPI.Application.Helpers;
using GestionSucursalesAPI.Infraestructure.Repository;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Queries.GetSucursal
{
    public class GetSucursalQueryHandler : IRequestHandler<GetSucursalRequest, GetSucursalResponse>
    {
        private readonly SucursalesDbContext _db;
        private readonly IMapper _mapper;

        public GetSucursalQueryHandler(SucursalesDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetSucursalResponse> Handle(GetSucursalRequest request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Sucursal> sucursals = _db.Sucursales;

            if (request.Id > 0)
            {
                sucursals = sucursals.Where(u => u.Id == request.Id);
            }

            var result = await sucursals
                .OrderByDescending(u => u.Id)
                .ProjectTo<GetSucursalDTO>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return new GetSucursalResponse()
            {
                Data = result,
                PageIndex = result.PageIndex,
                TotalPages = result.TotalPages,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage,
                TotalRecords = result.TotalRecords
            };
        }
    }
}
