namespace GestionSucursalesAPI.Infraestructure.Services.AuditService
{
    public interface IAuditService
    {
        public void Audit(AuditAction action, string message, object aditionalData);
        public void Create(string entityName, string entityId, object aditionalData);
        public void Update(string entityName, string entityId, object aditionalData);
        public void Delete(string entityName, string entityId, object aditionalData);
        public void Login(object aditionalData);
        public void Get(string entityName, string entityId, object aditionalData);
    }
}
