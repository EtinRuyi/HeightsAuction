using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Domain;
using HeightsAuction.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HeightsAuction.Application.ServicesImplementations
{
    public class BiddingService : IBiddingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BiddingService> _logger;

        public BiddingService(IUnitOfWork unitOfWork,
            IMapper mapper, ILogger<BiddingService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        //public async Task<ApiResponse<AddBidResponseDto>> AddBidAsync(string userId, AddBidRequestDto requestDto)
        //{
        //    try
        //    {
        //        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        //        if (user == null)
        //        {
        //            return ApiResponse<AddBidResponseDto>.Failed(false, "User does not exist", 404, new List<string> { });
        //        }

        //        var biddingRoom = await _unitOfWork.BiddingRooms.FindRooms(b => b.ItemId == requestDto.ItemId && b.AuctionEndDate > DateTime.UtcNow && !b.HasFinished);
        //        if (biddingRoom == null || biddingRoom.Count == 0)
        //        {
        //            return ApiResponse<AddBidResponseDto>.Failed(false, "Invalid bidding room or the room has finished", 400, new List<string> { });
        //        }

        //        var itemBiddingRoom = biddingRoom.First();
        //        var existingBid = await _unitOfWork.Bids.FindAsync(b => b.UserId == userId && b.ItemId == requestDto.ItemId);
        //        if (existingBid.Count > 0)
        //        {
        //            return ApiResponse<AddBidResponseDto>.Failed(false, "User has already placed a bid on this item", 400, new List<string> { });
        //        }

        //        var bid = new Bid
        //        {
        //            Amount = requestDto.Amount,
        //            ItemId = requestDto.ItemId,
        //            BiddingRoomId = itemBiddingRoom.Id,
        //            UserId = userId,
        //            BidTime = DateTime.UtcNow
        //        };

        //        await _unitOfWork.Bids.AddAsync(bid);
        //        await _unitOfWork.SaveChangesAsync();

        //        var responseDto = _mapper.Map<AddBidResponseDto>(bid);

        //        return ApiResponse<AddBidResponseDto>.Success(responseDto, "Bid added successfully", 200);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error occurred while adding a bid: {ex}");
        //        return ApiResponse<AddBidResponseDto>.Failed(false, "Error occurred while adding a bid", 500, new List<string> { });
        //    }
        //}

        public async Task<ApiResponse<AddBidResponseDto>> GetWinningBidFromRoom(string roomId)
        {
            try
            {
                var winningBid = await _unitOfWork.Bids.FindAsync(b => b.BiddingRoomId == roomId && b.IsHeighestBid);

                if (winningBid.Count == 0)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "No winning bid found for the specified room", 404, new List<string>());
                }

                var responseDto = _mapper.Map<AddBidResponseDto>(winningBid.First());

                return ApiResponse<AddBidResponseDto>.Success(responseDto, "Winning bid retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting winning bid: {ex}");
                return ApiResponse<AddBidResponseDto>.Failed(false, "Error occurred while getting winning bid", 500, new List<string>());
            }
        }
    }
}
