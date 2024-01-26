using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiddingRoomController : ControllerBase
    {
        private readonly IBiddingRoomService _biddingRoomService;
        public BiddingRoomController(IBiddingRoomService biddingRoomService)
        {
            _biddingRoomService = biddingRoomService;
        }

        [HttpPost("Create-Bidding-Room")]
        public async Task<IActionResult>CreateBiddingRoomAsyc([FromBody] CreateRoomRequestDto requestDto)
        {
            return Ok(await _biddingRoomService.CreateBiddingRoomAsync(requestDto));
        }

        [HttpPost("Join-Bidding-Room")]
        public async Task<IActionResult>JoinBiddingRoomAsync(string userId, string roomId)
        {
            return Ok(await _biddingRoomService.JoinBiddingRoomAsync(userId, roomId));
        }

        [HttpGet("Get-Bidding-Room/{Id}")]
        public async Task<IActionResult>GetBiddingRoomById(string roomId)
        {
            return Ok(await _biddingRoomService.GetBiddingRoomByIdAsync(roomId));
        }
    }
}
