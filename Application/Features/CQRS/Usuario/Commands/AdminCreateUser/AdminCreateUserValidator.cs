using FluentValidation;
using GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.CreateUser;
using GestionSucursalesAPI.Domain.Enums;

namespace GestionSucursalesAPI.Application.Features.CQRS.Usuario.Commands.AdminCreateUser
{
    public class AdminCreateUserValidator : AbstractValidator<AdminCreateUserRequest>
    {
        public AdminCreateUserValidator()
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

            RuleFor(request => request.Rol)
                 .IsInEnum().WithMessage("El rol proporcionado no es válido.");

            RuleFor(request => request.Rol)
                           .Must(rol => rol == RolUsuario.Administrador || rol == RolUsuario.Usuario)
                           .WithMessage("El rol debe ser Administrador (1) o Usuario (2).");


        }
    }
}
