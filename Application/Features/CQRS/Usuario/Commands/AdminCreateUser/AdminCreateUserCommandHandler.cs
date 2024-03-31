using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.AuditService;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.AdminCreateUser
{
    public class AdminCreateUserCommandHandler : IRequestHandler<AdminCreateUserRequest, Response>
    {
        private readonly SucursalesDbContext _db;
        private readonly IUserIdentity _userIdentity;
        private readonly IAuditService _audit;

        public AdminCreateUserCommandHandler(SucursalesDbContext db, IUserIdentity userIdentity, IAuditService audit)
        {
            _db = db;
            _userIdentity = userIdentity;
            _audit = audit;
        }

        public async Task<Response> Handle(AdminCreateUserRequest request, CancellationToken cancellationToken)
        {
            // Iniciar una transacción
            using (
                var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var userExists = await _db.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);

                    if (userExists)
                    {
                        return new Response
                        {
                            Code = 400,
                            Message = "Ese UserName ya existe"
                        };
                    }

                    var newUser = new Domain.Entities.Usuario
                    {
                        Identificacion = request.Identificacion,
                        Nombres = request.Nombres,
                        Apellidos = request.Apellidos,
                        Username = request.Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                        Rol = request.Rol,
                        FechaCreacion = DateTime.UtcNow,
                        CreadoPor = _userIdentity.GetUserId()
                    };

                    _db.Users.Add(newUser);
                    await _db.SaveChangesAsync(cancellationToken);

                    // Auditoría
                    _audit.Create("User", newUser.Id.ToString(), newUser);

                    await transaction.CommitAsync(cancellationToken); // Confirma la transacción si todo es exitoso

                    return new Response
                    {
                        Code = 200,
                        Message = "Usuario creado exitosamente"
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken); // Si hay una excepción, se deshace la transacción

                    _audit.Create("User", "Error", ex.Message);

                    return new Response
                    {
                        Code = 500,
                        Message = "Error al crear el nuevo Usuario en la base de datos"
                    };
                }
            }
        }
    }
}
