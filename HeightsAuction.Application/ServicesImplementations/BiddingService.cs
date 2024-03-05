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
        private readonly IItemService _itemService;

        public BiddingService(IUnitOfWork unitOfWork,
            IMapper mapper, ILogger<BiddingService> logger,
            IItemService itemService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _itemService = itemService;
        }

        public async Task<ApiResponse<AddBidResponseDto>> AddBidAsync(string userId, string roomId, string itemId, AddBidRequestDto requestDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "User not found", 404, new List<string>());
                }

                var biddingRoom = await _unitOfWork.BiddingRooms.GetByIdAsync(roomId);
                if (biddingRoom == null)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "Bidding Room not found", 404, new List<string>());
                }

                var item = await _unitOfWork.Items.GetByIdAsync(itemId);
                if (item == null)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "Item not found", 404, new List<string>());
                }

                if (biddingRoom.AuctionStartDate > DateTime.UtcNow)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "Bidding has not started", 400, new List<string>());
                }

                if (!biddingRoom.Bidders.Any(b => b.Id == userId) || biddingRoom.HasFinished)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "User cannot place bid in this Bidding Room or Bidding Room is closed", 400, new List<string>());
                }

                if (requestDto.Amount <= item.StartingPrice)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "Bid Amount cannot be accepted, please increase Bid Amount", 400, new List<string>());
                }

                if (requestDto.Amount <= item.CurrentBidPrice)
                {
                    return ApiResponse<AddBidResponseDto>.Failed(false, "Bid Amount cannot be accepted, please increase Bid Amount", 400, new List<string>());
                }

                var bid = _mapper.Map<Bid>(requestDto);
                bid.UserId = userId;
                bid.ItemId = itemId;
                bid.BiddingRoomId = roomId;
                bid.BidTime = DateTime.UtcNow;
                bid.CreatedBy = userId;

                await _unitOfWork.Bids.AddAsync(bid);
                await _unitOfWork.SaveChangesAsync();

                await _itemService.UpdateCurrentBidPrice(item);

                var responseDto = _mapper.Map<AddBidResponseDto>(bid);
                responseDto.BidId = bid.Id;
                responseDto.ItemId = itemId;
                responseDto.CurrentBidPrice = item.CurrentBidPrice;

                return ApiResponse<AddBidResponseDto>.Success(responseDto, "Bid added successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding bid: {ex}");
                return ApiResponse<AddBidResponseDto>.Failed(false, "Error occurred while adding bid", 500, new List<string>());
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
                var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(roomId);
                if (biddingRoom == null)
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "No Bidding room found", 500, new List<string>());
                }

                if (biddingRoom.HasFinished)
                {
                    var winningBid = await _unitOfWork.Bids.FindAsync(bid => bid.BiddingRoomId == roomId && bid.IsHeighestBid);

                    if (winningBid != null && winningBid.Any())
                    {
                        var winningBidItem = winningBid.FirstOrDefault();
                        var responseDto = _mapper.Map<BidResponseDto>(winningBidItem);
                        return ApiResponse<BidResponseDto>.Success(responseDto, "Winning bid retrieved successfully", 200);
                    }
                    else
                    {
                        return ApiResponse<BidResponseDto>.Failed(false, "No winning bid found for the specified room.", 500, new List<string>());
                    }
                }
                else
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "Cannot determine winning bid yet; Bidding Room is still active.", 500, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting winning bid: {ex}");
                return ApiResponse<BidResponseDto>.Failed(false, "Error occurred while getting winning bid", 500, new List<string>());
            }
        }

        public async Task UpdateWinningBidsAsync()
        {
            try
            {
                var finishedRooms = await _unitOfWork.BiddingRooms.FindAsync(room => room.HasFinished);

                foreach (var room in finishedRooms)
                {
                    var bids = await _unitOfWork.Bids.FindAsync(bid => bid.BiddingRoomId == room.Id);
                    var winningBid = bids.OrderByDescending(bid => bid.Amount).FirstOrDefault();

                    if (winningBid != null)
                    {
                        winningBid.IsHeighestBid = true;
                        room.WinningBidId = winningBid.Id;
                        _unitOfWork.Bids.Update(winningBid);
                        _unitOfWork.BiddingRooms.Update(room);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating winning bids: {ex}");
            }
        }
    }
}
