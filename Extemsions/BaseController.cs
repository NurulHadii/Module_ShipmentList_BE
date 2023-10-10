using BooxApp.Entity.Tenant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BooxApp.Api.Extemsions
{
    public static class BaseControllerExtension 
    {
        
        public static AuthUserObject GetCurrentUserData(this ControllerBase controllerBase)
        {
            var claims = controllerBase.User;
            var check = new AuthUserObject
            {
                ClientId = claims.Claims.FirstOrDefault(x => x.Type == "ClientId")?.Value ?? "",
                ClientName = claims.Claims.FirstOrDefault(x => x.Type == "ClientName")?.Value ?? "",
                ClientCode = claims.Claims.FirstOrDefault(x => x.Type == "ClientCode")?.Value ?? "",                
                UserName = claims.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? "",
                Email = claims.Claims.FirstOrDefault(x => x.Type == "Email")?.Value ?? "",
                UserId = claims.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? "",
                ModuleManagerKey = claims.Claims.FirstOrDefault(x => x.Type == "ModuleManagerKey")?.Value ?? "",
                Tenant = claims.Claims.FirstOrDefault(x => x.Type == "Tenant")?.Value ?? "",
                //ListTenant = JsonConvert.DeserializeObject<List<TenantListDto>>(claims.Claims.FirstOrDefault(x => x.Type == "ListTenant")?.Value) ?? new List<TenantListDto>(),

            };

            return check;
        }

    }

    public class AuthUserObject
    {
        public string UserId;
        public string UserName;
        public string Email;
        public string ClientId;
        public string ClientName;
        public string ClientCode;
        public string ModuleManagerKey;
        public string SessionKey;
        public string Tenant { get; set; }
        //public List<TenantListDto> ListTenant { get; set;}
    }
}