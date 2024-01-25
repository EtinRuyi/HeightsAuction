using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Domain;
using HeightsAuction.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HeightsAuction.Application.ServicesImplementations
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserServices> _logger;

        public UserServices(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserServices> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponse<PageResult<IEnumerable<RegisterResponseDto>>>> GetAllUsersAsync(int page, int perPage)
        {
            try
            {
                var allUsers = await _unitOfWork.Users.GetUsersAsync();
                var pagedUsers = await Pagination<AppUser>.GetPager(
                    allUsers, 
                    perPage, 
                    page,
                    user => user.LastName,
                    user => user.Id.ToString());
                var pagedUserDtos = _mapper.Map<PageResult<IEnumerable<RegisterResponseDto>>>(pagedUsers);

                return ApiResponse<PageResult<IEnumerable<RegisterResponseDto>>>.Success(pagedUserDtos, "Users found.", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users by pagination. Page: {Page}, PerPage: {PerPage}", page, perPage);
                return ApiResponse<PageResult<IEnumerable<RegisterResponseDto>>>.Failed(false, "An error occurred while retrieving users by pagination.", 500, new List<string> { ex.Message });
            }
        }
    }
}
