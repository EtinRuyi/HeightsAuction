using HeightsAuction.Application.DTOs;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IAuthenticationServices
    {
        Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequest);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest);
    }
}
