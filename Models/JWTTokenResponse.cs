using BooxApp.Api.Controllers;
using BooxApp.Entity.Tenant;
using Newtonsoft.Json.Linq;

namespace BooxApp.Api.Models
{
    public class JWTTokenResponse
    {
        public JWTTokenResponse()
        {
            listTenant  = new List<TenantListDto>();
        }
        public string name{ get; set; }

        public string surname { get; set; }
        public string email { get; set; }
        public string moduleManagerKey { get; set; }
        public string? api_token { get; set; }
        public string activeTenant { get; set; }
        public JObject ExtendInfo { get; set; } 
        public string tenant { get; set; }
        public Guid SessionKey { get; set; }
        public List<TenantListDto> listTenant { get; set; }
    }
}