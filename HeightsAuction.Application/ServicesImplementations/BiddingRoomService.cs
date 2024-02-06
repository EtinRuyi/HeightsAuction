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
    public class BiddingRoomService : IBiddingRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BiddingRoomService> _logger;

        public BiddingRoomService(IUnitOfWork unitOfWork, 
            IMapper mapper, ILogger<BiddingRoomService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CreateRoomResponseDto>> CreateBiddingRoomAsync(string userId, CreateRoomRequestDto requestDto)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByIdAsync(userId);
                if (existingUser == null)
                {
                    return ApiResponse<CreateRoomResponseDto>.Failed(false, "User does not exist", 404, new List<string> { });
                }
                var existingBidRoom = await _unitOfWork.BiddingRooms.FindAsync(room => room.Title == requestDto.Title);
                if (existingBidRoom.Any())
                {
                    return ApiResponse<CreateRoomResponseDto>.Failed(false, "Bidding Room with the same title already exists", 404, new List<string> { });
                }

                var biddingRoom = _mapper.Map<BiddingRoom>(requestDto);
                biddingRoom.CreatedBy = userId;

                // Add the user as a bidder
                biddingRoom.Bidders.Add(existingUser);
                await _unitOfWork.BiddingRooms.AddAsync(biddingRoom);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<CreateRoomResponseDto>(biddingRoom);
                responseDto.RoomId = biddingRoom.Id;
                responseDto.CreatedBy = userId;
                return ApiResponse<CreateRoomResponseDto>.Success(responseDto, "Bidding Room created successfully", 200);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error occurred while creating Bidding Room: {ex}");
                return ApiResponse<CreateRoomResponseDto>.Failed(false, "Error occurred while creating Bidding Room", 500, new List<string> { });
            }
        }

        public async Task<ApiResponse<PageResult<IEnumerable<BiddingRoomDto>>>> GetAllBiddingRoomsAsync(int page, int perPage)
        {
            try
            {
                var allRooms = await _unitOfWork.BiddingRooms.GetAllRoomssAsync();
                if (allRooms == null)
                {
                    return ApiResponse<PageResult<IEnumerable<BiddingRoomDto>>>.Failed(false, $"No Bidding Room found", 404, new List<string>());
                }

                // Fetch related entities for each room
                var roomsWithRelatedEntities = new List<BiddingRoom>();
                foreach (var room in allRooms)
                {
                    roomsWithRelatedEntities.Add(await _unitOfWork.BiddingRooms.IncludeRelatedEntities(room));
                }

                var pagedRooms = await Pagination<BiddingRoom>.GetPager(
                    roomsWithRelatedEntities,
                    perPage,
                    page,
                    biddingRoom => biddingRoom.Title,
                    biddingRoom => biddingRoom.Id.ToString());

                var pagedRoomDtos = _mapper.Map<PageResult<IEnumerable<BiddingRoomDto>>>(pagedRooms);
                return ApiResponse<PageResult<IEnumerable<BiddingRoomDto>>>.Success(pagedRoomDtos, "Rooms found.", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving rooms by pagination. Page: { Page}, PerPage: { PerPage} ", page, perPage);
                return ApiResponse<PageResult<IEnumerable<BiddingRoomDto>>>.Failed(false, "An error occurred while retrieving rooms by pagination.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<BiddingRoomDto>> GetBiddingRoomByIdAsync(string roomId)
        {
            try
            {
                var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(roomId);

                if (biddingRoom == null)
                {
                    return ApiResponse<BiddingRoomDto>.Failed(false, $"Bidding room with Id {roomId} not found", 404, new List<string>());
                }

                // Include related entities
                biddingRoom = await _unitOfWork.BiddingRooms.IncludeRelatedEntities(biddingRoom);

                var biddingRoomDto = _mapper.Map<BiddingRoomDto>(biddingRoom);
                return ApiResponse<BiddingRoomDto>.Success(biddingRoomDto, "Bidding room retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting a bidding room: {ex}");
                return ApiResponse<BiddingRoomDto>.Failed(false, "Error occurred while getting a bidding room", 500, new List<string>());
            }
        }


        public async Task<ApiResponse<JoinRoomResponseDto>> JoinBiddingRoomAsync(string userId, string roomId)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByIdAsync(userId);
                if (existingUser == null)
                {
                    return ApiResponse<JoinRoomResponseDto>.Failed(false, "User does not exist", 404, new List<string> { });
                }

                var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(roomId);
                if (biddingRoom == null)
                {
                    return ApiResponse<JoinRoomResponseDto>.Failed(false, "Bidding room does not exist", 404, new List<string> { });
                }

                if (biddingRoom.AuctionEndDate <= DateTime.UtcNow)
                {
                    biddingRoom.HasFinished = true;
                    return ApiResponse<JoinRoomResponseDto>.Failed(false, "Bidding room has closed", 400, new List<string> { });
                }

                biddingRoom.Bidders.Add(existingUser);
                _unitOfWork.BiddingRooms.Update(biddingRoom);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<JoinRoomResponseDto>(biddingRoom);
                return ApiResponse<JoinRoomResponseDto>.Success(responseDto, "User joined bidding room successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while joining bidding room: {ex}");
                return ApiResponse<JoinRoomResponseDto>.Failed(false, "Error occurred while joining bidding room", 500, new List<string> { });
            }
        }

    }
}
