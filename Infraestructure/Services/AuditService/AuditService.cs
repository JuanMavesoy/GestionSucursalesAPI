using GestionSucursalesAPI.Domain.Entities;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using Newtonsoft.Json;

namespace GestionSucursalesAPI.Infraestructure.Services.AuditService
{
    public class AuditService : IAuditService
    {
        private IUserIdentity _userIdentity;
        private  SucursalesDbContext _db;
        private ILogger<AuditService> _logger;

        public AuditService(SucursalesDbContext db, IUserIdentity userIdentity, ILogger<AuditService> logger)
        {
            _db = db;
            _userIdentity = userIdentity;
            _logger = logger;
        }

        public void Audit(AuditAction action, string message, object aditionalData)
        {
            AuditLog log = new AuditLog()
            {
                Application = _userIdentity.GetAppIdentifier() ?? "SucursalesAPI",
                Action = action.ToString() ?? "_",
                Message = string.IsNullOrEmpty(message) ? "_" : message,
                AditionalData = JsonConvert.SerializeObject(aditionalData) ?? "_",
                AuthProvider = _userIdentity.GetAuthProvider() ?? "_",
                CreatedAt = DateTime.Now,
                CreatedBy = _userIdentity.GetUserId()
            };

            try
            {
                _db.AuditLogs.Add(log);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al guardar en auditoria. Ex: " + ex.Message);
            }
        }

        public void Create(string entityName, string entityId, object aditionalData)
        {
            string message = string.Format("{0} (id: {1}) creado por {2}", entityName.ToUpper(), entityId, _userIdentity.GetUsername());
            Audit(AuditAction.Create, message, aditionalData);
        }

        public void Delete(string entityName, string entityId, object aditionalData)
        {
            string message = string.Format("{0} (id: {1}) eliminado por {2}", entityName.ToUpper(), entityId, _userIdentity.GetUsername());
            Audit(AuditAction.Delete, message, aditionalData);
        }

        public void Get(string entityName, string entityId, object aditionalData)
        {
            string message = string.Format("{0} (id: {1}) accedido por {2}", entityName.ToUpper(), entityId, _userIdentity.GetUsername());
            Audit(AuditAction.Get, message, aditionalData);
        }

        public void Login(object aditionalData)
        {
            string message = string.Format("Session iniciada por {0}", _userIdentity.GetUsername());
            Audit(AuditAction.Login, message, aditionalData);
        }

        public void Update(string entityName, string entityId, object aditionalData)
        {
            string message = string.Format("{0} (id: {1}) actualizado por {2}", entityName.ToUpper(), entityId, _userIdentity.GetUsername());
            Audit(AuditAction.Update, message, aditionalData);
        }
    }
}
