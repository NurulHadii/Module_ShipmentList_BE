using Microsoft.AspNetCore.Mvc;
using BooxApp.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BooxApp.Entity.Models;
using BooxApp.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using BooxApp.Core.Services.Interface;
using BooxApp.Core.Common;
using BooxApp.Api.Extemsions;
using BooxApp.Entity.Tenant;
using Newtonsoft.Json.Linq;
using BooxApp.Core.AuditTrail;
using BooxApp.Core.Model;

namespace BooxApp.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly BooxAppContext _context;
        private readonly IAuditRepository _auditRepository;
        private readonly IModuleRegistryService _moduleRegistryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;
        public AuthenticationController(BooxAppContext context, IModuleRegistryService moduleRegistryService, IHttpContextAccessor httpContextAccessor, IAuditRepository auditRepository, IAuthService authService)
        {
            _context = context;
            _moduleRegistryService = moduleRegistryService;
            _httpContextAccessor = httpContextAccessor;
            _auditRepository = auditRepository;
            _authService = authService;
        }

        [HttpPost("verify_token")]
        public async Task<IActionResult> verify_token([FromBody] LoginDto api_token)
        {
            if (api_token == null)
                return Unauthorized();

            var token = await _authService.ClaimToken(api_token.api_token);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new ApiOkResponse(token));


        }



        [HttpPost("refresh_token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto api_token)
        {
            if (api_token == null)
                return Unauthorized();

            var token = await _authService.verifyToken(api_token.access_token, api_token.refresh_token, Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new ApiOkResponse(token));

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto user)
        {
            try
            {
                if (user is null)
                {
                    return BadRequest("Invalid user request!!!");
                }


                var userAuth = await _authService.Login(user.email, user.Password, Request.HttpContext.Connection.RemoteIpAddress?.ToString());

                if (userAuth == null)
                    return Unauthorized();

                return Ok(new ApiOkResponse(userAuth));

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(AppUser appUser)
        {
            try
            {
                var secretKey = "BooxApp2023";
                var encrypt = appUser.Password.Encrypt(secretKey);
                //var decrtpy = encrypt.Decrypt(secretKey);

                appUser.Password = encrypt;
                _context.AppUser.Add(appUser);

                _context.SaveChanges();

                return Ok(new ApiOkResponse(appUser));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("SessionToken")]
        public async Task<IActionResult> SessionToken(string sessionKey)
        {
            try
            {
                var data = _context.AppUserSession.FirstOrDefault(x => x.SessionKey == new Guid(sessionKey));
                if (data == null)
                {
                    return Ok(new ApiResponse(400, "Not Found"));
                }

                var tokenExpired = _authService.GetPrincipalFromExpiredToken(data.Token);
                if (tokenExpired == null)
                {

                    return Ok(new ApiResponse(400, "Token Expired"));
                }

                //var jObjTokenExpired = JObject.FromObject(tokenExpired);
                //jObjTokenExpired.Remove("ExtendInfo");
                //jObjTokenExpired.Remove("listTenant");

                var result = JObject.Parse(data.ExtendInfo);
                result["Token"] = data.Token;
                result["SessionKey"] = new Guid(sessionKey);
                result["Tenant"] = tokenExpired.Claims.FirstOrDefault(x => x.Type == "Tenant")?.Value ?? "";
                //result["AuthData"] = jObjTokenExpired;


                return Ok(new ApiOkResponse(result));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
