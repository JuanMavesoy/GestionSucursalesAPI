namespace GestionSucursalesAPI.Infraestructure.Services.IdentityService
{
    public interface IUserIdentity
    {
        public string GetJwtBearer();
        public int GetUserId();
        public string GetUsername();
        public string GetName();
        public string GetProfile();
        public string GetAuthProvider();
        public string GetAppIdentifier();
        public bool IsProfileAuthorized(string profile);
        public bool IsProfileAuthorized(string profile, string appId);
        public bool IsAppAuthorized(string appId);
    }
}
