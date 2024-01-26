using HeightsAuction.Application.DTOs;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IBiddingRoomService
    {
        Task<ApiResponse<CreateRoomResponseDto>> CreateBiddingRoomAsync(CreateRoomRequestDto requestDto);
        Task<ApiResponse<JoinRoomResponseDto>> JoinBiddingRoomAsync(string userId, string roomId);
        Task<ApiResponse<BiddingRoomDto>> GetBiddingRoomByIdAsync(string roomId);
        //Task<ApiResponse<PageResult<IEnumerable<BiddingRoomDto>>>> GetAllBiddingRoomsAsync(int page, int perPage);
    }
}
