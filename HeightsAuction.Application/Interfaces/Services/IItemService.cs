using HeightsAuction.Application.DTOs;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IItemService
    {
        Task<ApiResponse<CreateItemResponseDto>> CreateItemAsync(string userId, string roomId, CreateItemRequestDto requestDto);
        Task<ApiResponse<PageResult<IEnumerable<ItemResponseDto>>>> GetAllItemsAsync(int page, int perPage);
    }
}
