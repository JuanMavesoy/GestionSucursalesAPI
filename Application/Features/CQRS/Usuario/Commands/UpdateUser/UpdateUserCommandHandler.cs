using AutoMapper;
using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.AuditService;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserRequest, Response>
    {
        private readonly SucursalesDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserIdentity _userIdentity;
        private readonly IAuditService _audit;


        public UpdateUserCommandHandler(SucursalesDbContext db, IMapper mapper, IUserIdentity userIdentity, IAuditService audit)
        {
            _db = db;
            _mapper = mapper;
            _userIdentity = userIdentity;
            _audit = audit;
        }

        public async Task<Response> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var existingUser = await _db.Users.FindAsync(request.Id);

                    if (existingUser == null)
                    {
                        return new Response
                        {
                            Code = 404,
                            Message = "Usuario no encontrado"
                        };
                    }

                    existingUser.Identificacion = request.Identificacion > 0 ? request.Identificacion : existingUser.Identificacion;


                    existingUser.Nombres = string.IsNullOrEmpty(request.Nombres) || request.Nombres.ToLower() == "string"
                                   ? existingUser.Nombres
                                   : request.Nombres;

                    existingUser.Apellidos = string.IsNullOrEmpty(request.Apellidos) || request.Apellidos.ToLower() == "string"
                                             ? existingUser.Apellidos
                                             : request.Apellidos;

                    existingUser.Username = string.IsNullOrEmpty(request.Username) || request.Username.ToLower() == "string"
                                            ? existingUser.Username
                                            : request.Username;

                    existingUser.Password = !string.IsNullOrEmpty(request.Password) && request.Password.ToLower() != "string"
                                            ? BCrypt.Net.BCrypt.HashPassword(request.Password)
                                            : existingUser.Password;

                    existingUser.FechaModificacion = DateTime.UtcNow;
                    existingUser.ModificadoPor = _userIdentity.GetUserId();


                    _db.Users.Update(existingUser);
                    await _db.SaveChangesAsync(cancellationToken);

                    // Auditoría
                    _audit.Update("User", existingUser.Id.ToString(), existingUser);

                    transaction.Commit(); // Confirma la transacción si todo es exitoso

                    return new Response
                    {
                        Code = 200,
                        Message = "Usuario actualizado exitosamente"
                    };
                }
                catch (DbUpdateException ex)
                {
                   await transaction.RollbackAsync(); // Si hay una excepción, se deshace la transacción

                    _audit.Create("User", "Error", ex.Message);

                    return new Response
                    {
                        Code = 500,
                        Message = "Error al actualizar el usuario en la base de datos"
                    };
                }
            }
        }
    }
}
