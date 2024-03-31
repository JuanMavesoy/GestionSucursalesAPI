using AutoMapper;
using AutoMapper.QueryableExtensions;
using GestionSucursalesAPI.Application.Helpers;
using GestionSucursalesAPI.Infraestructure.Repository;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        private readonly SucursalesDbContext _db;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(SucursalesDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Usuario> users = _db.Users;

            if (request.Id > 0)
            {
                users = users.Where(u => u.Id == request.Id);
            }

            var result = await users
                .OrderByDescending(u => u.Id)
                .ProjectTo<GetUsersDTO>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);


            return new GetUsersResponse()
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
