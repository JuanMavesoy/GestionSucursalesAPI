using FluentValidation;
using System.Text.RegularExpressions;

namespace GestionSucursalesAPI.Application.Features.CQRS.Sucursal.Commands.CreateSucursal
{
    public class CreateSucursalValidator : AbstractValidator<CreateSucursalRequest>
    {
        public CreateSucursalValidator()
        {
            RuleFor(request => request.Nombre)
                            .NotEmpty().WithMessage("El nombre de la sucursal es obligatorio.")
                            .MaximumLength(100).WithMessage("El nombre de la sucursal no puede tener más de 100 caracteres.");

            RuleFor(request => request.Direccion)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no puede tener más de 200 caracteres.");

            RuleFor(request => request.Telefono)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .Matches(new Regex(@"^\+?\d{10,15}$")).WithMessage("El teléfono no es válido."); // Regex simple para un número telefónico

            RuleFor(request => request.CorreoElectronico)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");

            RuleFor(request => request.HorarioAtencion)
                .NotEmpty().WithMessage("El horario de atención es obligatorio.");

            RuleFor(request => request.GerenteSucursal)
                .NotEmpty().WithMessage("El nombre del gerente de la sucursal es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del gerente de la sucursal no puede tener más de 100 caracteres.");

            RuleFor(request => request.Moneda)
                .IsInEnum().WithMessage("El valor para la moneda no es válido.");
        }
    }
}
