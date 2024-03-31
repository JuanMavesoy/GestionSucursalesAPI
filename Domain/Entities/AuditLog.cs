namespace GestionSucursalesAPI.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Application { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string AditionalData { get; set; }
        public string AuthProvider { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
