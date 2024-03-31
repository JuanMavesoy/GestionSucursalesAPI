using GestionSucursalesAPI.Application.DTOs;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.AuditService;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserRequest, Response>
    {
        private readonly SucursalesDbContext _db;
        private readonly IUserIdentity _userIdentity;
        private readonly IAuditService _audit;

        public DeleteUserCommandHandler(SucursalesDbContext db, IUserIdentity userIdentity, IAuditService auditService)
        {
            _db = db;
            _userIdentity = userIdentity;
            _audit = auditService;
        }

        public async Task<Response> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var userToDelete = await _db.Users.FindAsync(request.Id);

                if (userToDelete == null)
                {
                    return new Response
                    {
                        Code = 404,
                        Message = "Usuario no encontrado"
                    };
                }

                _db.Users.Remove(userToDelete);

                await _db.SaveChangesAsync(cancellationToken);

                // Auditoría
                _audit.Delete("User", userToDelete.Id.ToString(), userToDelete);

                return new Response
                {
                    Code = 200,
                    Message = "Usuario eliminado exitosamente"
                };

            }
            catch (Exception ex)
            {
                _audit.Delete("User", "Error", ex.Message);

                return new Response
                {
                    Code = 500,
                    Message = "Error al eliminar el usuario en la base de datos"
                };
            }
        }
    }
}
