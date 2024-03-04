using HeightsAuction.Application.DTOs;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IBiddingService
    {
        Task<ApiResponse<AddBidResponseDto>> AddBidAsync(string userId,string roomId, string itemId, AddBidRequestDto requestDto);
        Task<ApiResponse<BidResponseDto>> GetWinningBidFromRoom(string roomId);
        Task<ApiResponse<IEnumerable<BidResponseDto>>> GetAllBidAsync();
        Task UpdateWinningBidsAsync();
    }
}
