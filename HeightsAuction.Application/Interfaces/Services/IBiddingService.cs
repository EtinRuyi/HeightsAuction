using HeightsAuction.Application.DTOs;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IBiddingService
    {
        //Task<ApiResponse<AddBidResponseDto>> AddBidAsync(string userId, AddBidRequestDto requestDto);
        Task<ApiResponse<AddBidResponseDto>> GetWinningBidFromRoom(string roomId);
    }
}
