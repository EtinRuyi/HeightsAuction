using HeightsAuction.Application.DTOs;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IBiddingRoomService
    {
        Task<ApiResponse<CreateRoomResponseDto>> CreateBiddingRoomAsync(string userId, CreateRoomRequestDto requestDto);
        Task<ApiResponse<JoinRoomResponseDto>> JoinBiddingRoomAsync(string userId, string roomId);
        Task<ApiResponse<BiddingRoomDto>> GetBiddingRoomByIdAsync(string roomId);
        Task<ApiResponse<PageResult<IEnumerable<BiddingRoomDto>>>> GetAllBiddingRoomsAsync(int page, int perPage);
    }
}
