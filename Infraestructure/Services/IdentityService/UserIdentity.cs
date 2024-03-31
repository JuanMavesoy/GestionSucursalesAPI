using System.Security.Claims;

namespace GestionSucursalesAPI.Infraestructure.Services.IdentityService
{
    public class UserIdentity : IUserIdentity
    {
        private ClaimsPrincipal _claim;
        private IHttpContextAccessor _contextAccessor;
        public UserIdentity(IHttpContextAccessor httpAccessor)
        {
            _contextAccessor = httpAccessor;
            _claim = _contextAccessor?.HttpContext?.User;
        }


        public string GetAppIdentifier()
        {
            return _claim.Claims.FirstOrDefault(f => f.Type == "appId")?.Value.Trim().ToUpper();
        }

        public string GetAuthProvider()
        {
            return _claim.Claims.FirstOrDefault(f => f.Type == "authProvider")?.Value.Trim();
        }

        public string GetJwtBearer()
        {
            return _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
        }

        public string GetName()
        {
            return _claim.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name)?.Value.Trim().ToUpper();
        }

        public string GetProfile()
        {
            return _claim.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Role)?.Value.Trim().ToUpper();
        }

        public int GetUserId()
        {
            return Convert.ToInt32(_claim.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value);
        }

        public string GetUsername()
        {
            return _claim.Claims.FirstOrDefault(f => f.Type == ClaimTypes.UserData)?.Value.Trim().ToUpper();
        }

        public bool IsAppAuthorized(string appId)
        {
            return GetAppIdentifier().Equals(appId.Trim().ToUpper());
        }

        public bool IsProfileAuthorized(string profile)
        {
            return GetProfile().Equals(profile.Trim().ToUpper());
        }

        public bool IsProfileAuthorized(string profile, string appId)
        {
            return GetProfile().Equals(profile.Trim().ToUpper()) && GetAppIdentifier().Equals(appId.Trim().ToUpper());
        }
    }
}
