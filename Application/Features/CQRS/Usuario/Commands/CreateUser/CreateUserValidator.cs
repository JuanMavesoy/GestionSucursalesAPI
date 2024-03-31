using FluentValidation;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(request => request.Identificacion)
                       .GreaterThan(0).WithMessage("La identificación debe ser mayor que cero.");

            RuleFor(request => request.Nombres)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres.");

            RuleFor(request => request.Apellidos)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(50).WithMessage("El apellido no puede tener más de 50 caracteres.");

            RuleFor(request => request.Username)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de usuario no puede tener más de 50 caracteres.");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

        }
    }
}
