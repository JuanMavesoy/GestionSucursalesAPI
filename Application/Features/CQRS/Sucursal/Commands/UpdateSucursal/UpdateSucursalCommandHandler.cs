using AutoMapper;
using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.AuditService;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.UpdateSucursal
{
    public class UpdateSucursalCommandHandler : IRequestHandler<UpdateSucursalRequest, Response>
    {

        private readonly SucursalesDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserIdentity _userIdentity;
        private readonly IAuditService _audit;

        public UpdateSucursalCommandHandler(SucursalesDbContext db, IMapper mapper, IUserIdentity userIdentity, IAuditService audit)
        {
            _db = db;
            _mapper = mapper;
            _userIdentity = userIdentity;
            _audit = audit;
        }

        public async Task<Response> Handle(UpdateSucursalRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var existingSucursal = await _db.Sucursales.FindAsync(request.Id);

                    if (existingSucursal == null)
                    {
                        return new Response
                        {
                            Code = 404,
                            Message = "Sucursal no encontrada."
                        };
                    }
                   
                    existingSucursal.Nombre = !string.IsNullOrEmpty(request.Nombre) && request.Nombre.ToLower() != "string"
                                              ? request.Nombre
                                              : existingSucursal.Nombre;

                    existingSucursal.Direccion = !string.IsNullOrEmpty(request.Direccion) && request.Direccion.ToLower() != "string"
                                                 ? request.Direccion
                                                 : existingSucursal.Direccion;

                    existingSucursal.Telefono = !string.IsNullOrEmpty(request.Telefono) && request.Telefono.ToLower() != "string"
                                                ? request.Telefono
                                                : existingSucursal.Telefono;

                    existingSucursal.CorreoElectronico = !string.IsNullOrEmpty(request.CorreoElectronico) && request.CorreoElectronico.ToLower() != "string"
                                                         ? request.CorreoElectronico
                                                         : existingSucursal.CorreoElectronico;

                    existingSucursal.HorarioAtencion = !string.IsNullOrEmpty(request.HorarioAtencion) && request.HorarioAtencion.ToLower() != "string"
                                                       ? request.HorarioAtencion
                                                       : existingSucursal.HorarioAtencion;

                    existingSucursal.GerenteSucursal = !string.IsNullOrEmpty(request.GerenteSucursal) && request.GerenteSucursal.ToLower() != "string"
                                                       ? request.GerenteSucursal
                                                       : existingSucursal.GerenteSucursal;


                    existingSucursal.Moneda = (request.Moneda == Domain.Enums.Moneda.USD || request.Moneda == Domain.Enums.Moneda.COP)
                                     ? request.Moneda
                                     : existingSucursal.Moneda;

                    existingSucursal.FechaUltimaActualizacion = DateTime.UtcNow;
                    existingSucursal.ModificadoPor = _userIdentity.GetUserId();

                    _db.Sucursales.Update(existingSucursal);
                    await _db.SaveChangesAsync(cancellationToken);

                    // Auditoría
                    _audit.Update("Sucursal", existingSucursal.Id.ToString(), existingSucursal);

                    await transaction.CommitAsync(); // Confirma la transacción si todo es exitoso

                    return new Response
                    {
                        Code = 200,
                        Message = "Sucursal actualizada exitosamente."
                    };
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync(); // Si hay una excepción, se deshace la transacción

                    _audit.Create("Sucursal", "Error", ex.Message);

                    return new Response
                    {
                        Code = 500,
                        Message = "Error al actualizar la sucursal en la base de datos."
                    };
                }
            }
        }
    }
}
