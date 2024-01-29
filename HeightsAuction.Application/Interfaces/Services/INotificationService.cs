using HeightsAuction.Application.DTOs;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<ApiResponse<BidNotificationResponseDto>> NotifyParticipantsAsync(BidNotificationRequestDto requestDto);
        Task<ApiResponse<BidNotificationResponseDto>> NotifyAuctionConclusionAsync(BidNotificationRequestDto requestDto);
    }
}
