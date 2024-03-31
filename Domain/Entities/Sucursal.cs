namespace GestionSucursalesAPI.Domain.Entities
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string HorarioAtencion { get; set; }
        public string GerenteSucursal { get; set; }
        public Enums.Moneda Moneda { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? CreadoPor { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
        public int ModificadoPor { get; set; }

    }
}
