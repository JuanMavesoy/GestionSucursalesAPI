using GestionSucursalesAPI.Domain.Enums;

namespace GestionSucursalesAPI.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public long Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public RolUsuario Rol { get; set; }

        public DateTime FechaCreacion { get; set; }
        public int CreadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int ModificadoPor { get; set; }
    }
}
