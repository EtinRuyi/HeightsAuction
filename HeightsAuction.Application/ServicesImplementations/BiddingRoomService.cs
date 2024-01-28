using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
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

        //TODO: An ITEM should be created during room creation. This is helpful when a user wants to place a bid, to include the ItemId
        // Updated BiddingRoomService
        public async Task<ApiResponse<CreateRoomResponseDto>> CreateBiddingRoomAsync(CreateRoomRequestDto requestDto)
        {
            try
            {
                var existingRoom = await _unitOfWork.BiddingRooms.FindRooms(b => b.Title == requestDto.Title);
                if (existingRoom != null)
                {
                    return ApiResponse<CreateRoomResponseDto>.Failed(false, "Bidding room with the same name already exists", 400, new List<string> { });
                }

                var item = new Item
                {
                    Name = requestDto.ItemName,
                    Description = requestDto.ItemDescription,
                    StartingPrice = requestDto.ItemStartingPrice,
                    CurrentBidPrice = requestDto.ItemStartingPrice
                };

                var biddingRoom = new BiddingRoom
                {
                    Title = requestDto.Title,
                    Item = item,
                    AuctionStartDate = requestDto.AuctionStartDate,
                    AuctionEndDate = requestDto.AuctionEndDate,
                };

                await _unitOfWork.BiddingRooms.CreateRoomAsync(biddingRoom);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<CreateRoomResponseDto>(biddingRoom);
                responseDto.ItemId = item.Id; 

                return ApiResponse<CreateRoomResponseDto>.Success(responseDto, "Bidding room created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding a board: {ex}");
                return ApiResponse<CreateRoomResponseDto>.Failed(false, "Error occurred while adding a board", 500, new List<string> { });
            }
        }


        //public async Task<ApiResponse<CreateRoomResponseDto>> CreateBiddingRoomAsync(CreateRoomRequestDto requestDto)
        //{
        //    try
        //    {
        //        var existingroom = await _unitOfWork.BiddingRooms.FindRooms(b => b.Title == requestDto.Title);
        //        if (existingroom != null)
        //        {
        //            return ApiResponse<CreateRoomResponseDto>.Failed(false, "Bidding room with the same name already exists", 400, new List<string> { });
        //        }

        //        var biddingRoom = _mapper.Map<BiddingRoom>(requestDto);
        //        await _unitOfWork.BiddingRooms.CreateRoomAsync(biddingRoom);
        //        await _unitOfWork.SaveChangesAsync();

        //        var responseDto = _mapper.Map<CreateRoomResponseDto>(biddingRoom);

        //        return ApiResponse<CreateRoomResponseDto>.Success(responseDto, "Bidding room created successfully", 201);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error occurred while adding a board: {ex}");
        //        return ApiResponse<CreateRoomResponseDto>.Failed(false, "Error occurred while adding a board", 500, new List<string> { });
        //    }
        //}

        public async Task<ApiResponse<BiddingRoomDto>> GetBiddingRoomByIdAsync(string roomId)
        {
            try
            {
                var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(roomId);

                if (biddingRoom == null)
                {
                    return ApiResponse<BiddingRoomDto>.Failed(false, $"Bidding room with Id {roomId} not found", 404, new List<string>());
                }

                var bidders = biddingRoom.Bidders.ToList();
                var biddingRoomDto = _mapper.Map<BiddingRoomDto>(biddingRoom);
                biddingRoomDto.Bidders = _mapper.Map<ICollection<AppUser>>(bidders);

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

                //if (biddingRoom.HasFinished || biddingRoom.AuctionEndDate <= DateTime.UtcNow)
                //{
                //    return ApiResponse<JoinRoomResponseDto>.Failed(false, "Bidding room has closed", 400, new List<string> { });
                //}

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
