using GestionSucursalesAPI.Application.Features.CQRS.Auth.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionSucursalesAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _mediator.Send(request);

            if (token == null)
            {
                // El inicio de sesión falló
                return Unauthorized();
            }

            // El inicio de sesión fue exitoso, devolver el token
            return Ok(new { Token = token });
        }
    }
}
