using GestionSucursalesAPI.Application.DTOs;
using MediatR;
using System;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.CreateSucursal
{
    public class CreateSucursalRequest : IRequest<Response>
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string HorarioAtencion { get; set; }
        public string GerenteSucursal { get; set; }
        public Domain.Enums.Moneda Moneda { get; set; }
    }
}
