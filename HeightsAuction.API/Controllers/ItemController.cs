using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService) 
        {
            _itemService = itemService;
        }

        [HttpPost("Create-Item")]
        public async Task<IActionResult>CreateItemAsync(string userId, string roomId, [FromBody] CreateItemRequestDto requestDto)
        {
            return Ok(await _itemService.CreateItemAsync(userId, roomId, requestDto));
        }

        [HttpGet("GetAllItem")]
        public async Task<IActionResult>GetAllItemsAsync(int page, int perPage)
        {
            return Ok(await _itemService.GetAllItemsAsync(page, perPage));
        }
    }
}
