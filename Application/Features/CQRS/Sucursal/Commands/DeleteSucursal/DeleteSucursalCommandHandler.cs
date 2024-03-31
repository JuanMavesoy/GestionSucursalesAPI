using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.AuditService;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.DeleteSucursal
{
    public class DeleteSucursalCommandHandler : IRequestHandler<DeleteSucursalRequest, Response>
    {
        private readonly SucursalesDbContext _db;
        private readonly IUserIdentity _userIdentity;
        private readonly IAuditService _audit;

        public DeleteSucursalCommandHandler(SucursalesDbContext db, IUserIdentity userIdentity, IAuditService auditService)
        {
            _db = db;
            _userIdentity = userIdentity;
            _audit = auditService;
        }

        public async Task<Response> Handle(DeleteSucursalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var sucursalToDelete = await _db.Sucursales.FindAsync(request.Id);

                if (sucursalToDelete == null)
                {
                    return new Response
                    {
                        Code = 404,
                        Message = "Sucursal no encontrada"
                    };
                }

                _db.Sucursales.Remove(sucursalToDelete);

                await _db.SaveChangesAsync(cancellationToken);

                // Auditoría
                _audit.Delete("Sucursal", sucursalToDelete.Id.ToString(), sucursalToDelete);

                return new Response
                {
                    Code = 200,
                    Message = "Sucursal eliminada exitosamente"
                };

            }
            catch (Exception ex)
            {
                _audit.Delete("Sucursal", "Error", ex.Message);

                return new Response
                {
                    Code = 500,
                    Message = "Error al eliminar la sucursal en la base de datos"
                };
            }
        }
    }
}
