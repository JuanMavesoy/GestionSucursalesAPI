using MediatR;

namespace GestionSucursalesAPI.Application.Features.CQRS.Auth.Commands.Login
{
    public class LoginRequest : IRequest<string>
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
