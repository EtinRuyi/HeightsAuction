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

        public async Task<ApiResponse<BidResponseDto>> AddBidAsync(string userId, string roomId, string itemId, AddBidRequestDto requestDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "User not found", 404, new List<string>());
                }

                var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(roomId);
                if (biddingRoom == null)
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "Bidding Room not found", 404, new List<string>());
                }

                var item = await _unitOfWork.Items.GetByIdAsync(itemId);
                if (item == null)
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "Item not found", 404, new List<string>());
                }

                if (!biddingRoom.Bidders.Any(b => b.Id == userId) || biddingRoom.HasFinished)
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "User cannot place bid in this room", 400, new List<string>());
                }

                var bid = _mapper.Map<Bid>(requestDto);
                bid.UserId = userId;
                bid.ItemId = itemId;
                bid.BiddingRoomId = roomId;
                bid.BidTime = DateTime.UtcNow;
                bid.CreatedBy = userId;

                await _unitOfWork.Bids.AddAsync(bid);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<BidResponseDto>(bid);
                responseDto.ItemId = itemId;

                return ApiResponse<BidResponseDto>.Success(responseDto, "Bid added successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding bid: {ex}");
                return ApiResponse<BidResponseDto>.Failed(false, "Error occurred while adding bid", 500, new List<string>());
            }
        }


        public async Task<ApiResponse<IEnumerable<BidResponseDto>>> GetAllBidAsync()
        {
            try
            {
                var allBids = await _unitOfWork.Bids.GetAllAsync();
                if (allBids == null)
                {
                    return ApiResponse<IEnumerable<BidResponseDto>>.Failed(false, "No Bids found", 400, new List<string> { });
                }

                var BidsDto = _mapper.Map<IEnumerable<BidResponseDto>>(allBids);
                return ApiResponse<IEnumerable<BidResponseDto>>.Success(BidsDto, "Bids found", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while retrieving Bids");
                return ApiResponse<IEnumerable<BidResponseDto>>.Failed(false, "An error occured while retrieving Bids", 400, new List<string> { });
            }
        }

        public async Task<ApiResponse<BidResponseDto>> GetWinningBidFromRoom(string roomId)
        {
            try
            {
                var winningBid = await _unitOfWork.Bids.FindAsync(b => b.BiddingRoomId == roomId && b.IsHeighestBid);

                if (winningBid.Count == 0)
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "No winning bid found for the specified room", 404, new List<string>());
                }

                var responseDto = _mapper.Map<BidResponseDto>(winningBid.First());

                return ApiResponse<BidResponseDto>.Success(responseDto, "Winning bid retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting winning bid: {ex}");
                return ApiResponse<BidResponseDto>.Failed(false, "Error occurred while getting winning bid", 500, new List<string>());
            }
        }
    }
}
