using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _services;
        public AuthenticationController(IAuthenticationServices services)
        {
            _services = services;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _services.RegisterAsync(registerRequest));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _services.LoginAsync(loginRequest));

        }
    }
}
