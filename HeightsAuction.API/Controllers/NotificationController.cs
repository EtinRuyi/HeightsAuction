using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("notifyParticipants")]
        public async Task<IActionResult> NotifyParticipants([FromBody] BidNotificationRequestDto requestDto)
        {
            var result = await _notificationService.NotifyParticipantsAsync(requestDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("notifyAuctionConclusion")]
        public async Task<IActionResult> NotifyAuctionConclusion([FromBody] BidNotificationRequestDto requestDto)
        {
            var result = await _notificationService.NotifyAuctionConclusionAsync(requestDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
