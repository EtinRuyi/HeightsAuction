using HeightsAuction.Application.DTOs;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IBiddingService
    {
        Task<ApiResponse<BidResponseDto>> AddBidAsync(string userId,string roomId, string itemId, AddBidRequestDto requestDto);
        Task<ApiResponse<BidResponseDto>> GetWinningBidFromRoom(string roomId);
        Task<ApiResponse<PageResult<IEnumerable<BidResponseDto>>>> GetAllBidAsync(int page, int perPage);
    }
}
