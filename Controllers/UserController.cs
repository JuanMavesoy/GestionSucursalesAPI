using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.AdminCreateUser;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.CreateUser;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.DeleteUser;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.UpdateUser;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSucursalesAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser(int id, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var request = new GetUsersRequest()
                {
                    Id = id,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                _logger.LogInformation("Iniciando la obtención de Usuarios @{GetUsersRequest}", request);

                var result = await _mediator.Send(request);

                _logger.LogInformation("Obtención de usuario exitosa @{GetUsersRequest}", request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener usuario: {ex.Message}");

                return StatusCode(500, "Error interno del servidor");
            }
        }


        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando el proceso de creación de usuario @{CreateUserRequest}", request);

                var response = await _mediator.Send(request);

                _logger.LogInformation($"Usuario creado exitosamente @{{CreateUserRequest}}: Code={response.Code}, Message={response.Message}", request);

                return StatusCode(response.Code, new { Message = response.Message });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud @{{CreateUserRequest}}: {ex.Message}", request);

                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }


        [Authorize]
        [HttpPost("AdministradorCrearUsuario")]
        public async Task<IActionResult> AdminCreateUser(AdminCreateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando el proceso de creación de usuario @{CreateUserRequest}", request);

                var response = await _mediator.Send(request);

                _logger.LogInformation($"Usuario creado exitosamente @{{CreateUserRequest}}: Code={response.Code}, Message={response.Message}", request);

                return StatusCode(response.Code, new { Message = response.Message });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud @{{CreateUserRequest}}: {ex.Message}", request);

                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando el proceso de actualizacion de usuario @{UpdateUserRequest}", request);

                var response = await _mediator.Send(request);

                _logger.LogInformation($"Usuario actualizadp exitosamente @{{UpdateUserRequest}}: Code={response.Code}, Message={response.Message}", request);

                return StatusCode(response.Code, new { Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud @{{UpdateUserRequest}}: {ex.Message}", request);

                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }


        [Authorize]
        [HttpDelete("EliminarUsuario/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var request = new DeleteUserRequest { Id = id };

                _logger.LogInformation("Se recibió una solicitud para eliminar un usuario @{DeleteUserRequest}", request);

                var response = await _mediator.Send(request);

                _logger.LogInformation($"Respuesta de la eliminación de usuario @{{DeleteUserRequest}}: Code={response.Code}, Message={response.Message}", request);

                return StatusCode(response.Code, new { Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud de eliminación @{{DeleteUserRequest}}: {ex.Message}");
                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }

    }
}
