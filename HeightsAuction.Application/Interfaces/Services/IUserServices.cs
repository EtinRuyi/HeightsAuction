using HeightsAuction.Application.DTOs;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<ApiResponse<PageResult<IEnumerable<RegisterResponseDto>>>> GetAllUsersAsync(int page, int perPage);
    }
}
