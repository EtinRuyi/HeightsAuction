using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiddingController : ControllerBase
    {
        private readonly IBiddingService _biddingService;
        public BiddingController(IBiddingService biddingService)
        {
            _biddingService = biddingService;
        }

        [HttpPost("AddBid")]
        public async Task<IActionResult> AddBidAsync(string userId, string roomId, string itemId, [FromBody] AddBidRequestDto requestDto)
        {
            return Ok(await _biddingService.AddBidAsync(userId, roomId, itemId, requestDto));
        }

        [HttpGet("GetAllBids")]
        public async Task<IActionResult>GetAllBidsAsync()
        {
            return Ok(await _biddingService.GetAllBidAsync());
        }

        [HttpGet("GetWinningBid/{roomId}")]
        public async Task<IActionResult> GetWinningBid(string roomId)
        {
            var response = await _biddingService.GetWinningBidFromRoom(roomId);
            return Ok(response);
        }
        }
    }
