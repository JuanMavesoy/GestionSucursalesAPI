using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.AuditService;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.CreateSucursal
{
    public class CreateSucursalCommandHandler : IRequestHandler<CreateSucursalRequest, Response>
    {
        private readonly SucursalesDbContext _db;
        private readonly IUserIdentity _userIdentity;
        private readonly IAuditService _audit;

        public CreateSucursalCommandHandler(SucursalesDbContext db, IUserIdentity userIdentity, IAuditService auditService)
        {
            _db = db;
            _userIdentity = userIdentity;
            _audit = auditService;
        }

        public async Task<Response> Handle(CreateSucursalRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var sucursalExists = await _db.Sucursales.AnyAsync(u => u.Nombre == request.Nombre, cancellationToken);

                    if (sucursalExists)
                    {
                        return new Response
                        {
                            Code = 400,
                            Message = "El nombre de la sucursal ya existe"
                        };
                    }

                    var newSucursal = new Domain.Entities.Sucursal
                    {
                        Nombre = request.Nombre,
                        Direccion = request.Direccion,
                        Telefono = request.Telefono,
                        CorreoElectronico = request.CorreoElectronico,
                        HorarioAtencion = request.HorarioAtencion,
                        GerenteSucursal = request.GerenteSucursal,
                        Moneda = request.Moneda,
                        FechaCreacion = DateTime.UtcNow,
                        CreadoPor = _userIdentity.GetUserId()
                    };

                    _db.Sucursales.Add(newSucursal);
                    await _db.SaveChangesAsync(cancellationToken);

                    _audit.Create("Sucursal", newSucursal.Id.ToString(), newSucursal);

                    await transaction.CommitAsync(cancellationToken);

                    return new Response
                    {
                        Code = 200,
                        Message = "Sucursal creada exitosamente"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    _audit.Create("Sucursal", "Error", ex.Message);

                    return new Response
                    {
                        Code = 500,
                        Message = "Error al crear la nueva Sucursal en la base de datos"
                    };
                }
            }
        }
    }
}
