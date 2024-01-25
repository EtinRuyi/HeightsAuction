using HeightsAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet("Get-All-Users")]
        public async Task<IActionResult>GetAllUser(int page, int perPage)
        {
            return Ok(await _userServices.GetAllUsersAsync(page, perPage));
        }
    }
}
