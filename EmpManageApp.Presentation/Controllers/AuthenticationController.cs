using EmpManageApp.Domain.Entities;
using EmpManageApp.Presentation.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpManageApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserAuthenticationService _authService;

        public AuthenticationController(UserAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginModel credentials)
        {
            if (IsValidUser(credentials))
            {
                var token = _authService.GenerateToken(credentials.Id);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        private bool IsValidUser(LoginModel credentials)
        {
            return credentials.Id == "Id1" && credentials.Password == "Akansha";
        }
    }
}
