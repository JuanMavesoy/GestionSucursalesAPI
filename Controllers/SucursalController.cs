using GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.CreateSucursal;
using GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.DeleteSucursal;
using GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.UpdateSucursal;
using GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Queries.GetSucursal;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSucursalesAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public SucursalController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSucursal(int id, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var request = new GetSucursalRequest()
                {
                    Id = id,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                _logger.LogInformation("Iniciando la obtención de la Sucursal @{GetSucursalRequest}", request);

                var result = await _mediator.Send(request);

                _logger.LogInformation("Obtención de Sucursal exitosa @{GetSucursalRequest}", request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la sucursal: {ex.Message}");

                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize]
        [HttpPost("CrearSucursal")]
        public async Task<IActionResult> CreateSucursal(CreateSucursalRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando el proceso de creación de sucursal @{CreateSucursalRequest}", request);

                var response = await _mediator.Send(request);

                _logger.LogInformation($"Sucursal creada exitosamente @{{CreateSucursalRequest}}: Code={response.Code}, Message={response.Message}", request);

                return StatusCode(response.Code, new { Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud @{{CreateSucursalRequest}}: {ex.Message}", request);

                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }

        [Authorize]
        [HttpPut("ActualizarSucursal")]
        public async Task<IActionResult> UpdateSucursal(UpdateSucursalRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando el proceso de actualización de la sucursal @{UpdateSucursalRequest}", request);

                var response = await _mediator.Send(request);

                _logger.LogInformation($"Sucursal actualizada exitosamente @{{UpdateSucursalRequest}}: Code={response.Code}, Message={response.Message}", request);

                return StatusCode(response.Code, new { Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud @{{UpdateSucursalRequest}}: {ex.Message}", request);

                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }

        [Authorize]
        [HttpDelete("EliminarSucursal/{id}")]
        public async Task<IActionResult> DeleteSucursal(int id)
        {
            try
            {
                var request = new DeleteSucursalRequest { Id = id };

                _logger.LogInformation("Se recibió una solicitud para eliminar una sucursal @{DeleteSucursalRequest}", request);

                var response = await _mediator.Send(request);

                _logger.LogInformation($"Respuesta de la eliminación de sucursal @{{DeleteSucursalRequest}}: Code={response.Code}, Message={response.Message}", request);

                return StatusCode(response.Code, new { Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud de eliminación @{{DeleteSucursalRequest}}: {ex.Message}");
                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }

    }
}
