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

                if (!biddingRoom.Bidders.Any(b => b.Id == userId) || biddingRoom.AuctionEndDate <= DateTime.UtcNow)
                {
                    biddingRoom.HasFinished = true;
                    return ApiResponse<BidResponseDto>.Failed(false, "User cannot place bid in this Bidding Room or Bidding Room is closed", 400, new List<string>());
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
                Bid winningBid = null; // Declare here so it's accessible outside the if block

                var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(roomId);
                if (biddingRoom == null)
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "No Bidding room found", 500, new List<string>());
                }

                if (biddingRoom.AuctionEndDate <= DateTime.UtcNow && !biddingRoom.HasFinished)
                {
                    var bids = await _unitOfWork.Bids.FindAsync(b => b.BiddingRoomId == roomId);
                    if (bids.Any())
                    {
                        winningBid = bids.OrderByDescending(b => b.Amount).FirstOrDefault();

                        if (winningBid != null)
                        {
                            biddingRoom.WinningBidId = winningBid.Id;
                            winningBid.IsHeighestBid = true;
                            biddingRoom.HasFinished = true;

                            _unitOfWork.Bids.Update(winningBid);
                            _unitOfWork.BiddingRooms.Update(biddingRoom);
                            await _unitOfWork.SaveChangesAsync();
                        }
                        else
                        {
                            return ApiResponse<BidResponseDto>.Failed(false, "No winning bid found for the specified room.", 500, new List<string>());
                        }
                    }
                    else
                    {
                        return ApiResponse<BidResponseDto>.Failed(false, "No bids found for the specified room.", 500, new List<string>());
                    }
                }
                else
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "Cannot determine winning bid yet; Bidding Room is still active.", 500, new List<string>());
                }

                if (winningBid != null)
                {
                    var responseDto = _mapper.Map<BidResponseDto>(winningBid);
                    return ApiResponse<BidResponseDto>.Success(responseDto, "Winning bid retrieved successfully", 200);
                }
                else
                {
                    return ApiResponse<BidResponseDto>.Failed(false, "No winning bid found for the specified room.", 500, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting winning bid: {ex}");
                return ApiResponse<BidResponseDto>.Failed(false, "Error occurred while getting winning bid", 500, new List<string>());
            }
        }


        //public async Task<ApiResponse<BidResponseDto>> GetWinningBidFromRoom(string roomId)
        //{
        //    try
        //    {
        //        await UpdateWinningBid(roomId);

        //        var winningBid = await _unitOfWork.Bids.FindAsync(b => b.BiddingRoomId == roomId);

        //        if (!winningBid.Any())
        //        {
        //            return ApiResponse<BidResponseDto>.Failed(false, "No winning bid found for the specified room", 404, new List<string>());
        //        }

        //        var responseDto = _mapper.Map<BidResponseDto>(winningBid.First());

        //        return ApiResponse<BidResponseDto>.Success(responseDto, "Winning bid retrieved successfully", 200);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error occurred while getting winning bid: {ex}");
        //        return ApiResponse<BidResponseDto>.Failed(false, "Error occurred while getting winning bid", 500, new List<string>());
        //    }
        //}

        //public async Task UpdateWinningBid(string roomId)
        //{
        //    try
        //    {
        //        var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(roomId);
        //        if (biddingRoom == null)
        //        {
        //            _logger.LogError("Bidding room not found.");
        //            return;
        //        }

        //        if (biddingRoom.AuctionEndDate <= DateTime.UtcNow && !biddingRoom.HasFinished)
        //        {
        //            var bids = await _unitOfWork.Bids.FindAsync(b => b.BiddingRoomId == roomId);
        //            if (bids.Any())
        //            {
        //                var winningBid = bids.OrderByDescending(b => b.Amount).FirstOrDefault();

        //                if (winningBid != null)
        //                {
        //                    biddingRoom.WinningBidId = winningBid.Id;
        //                    winningBid.IsHeighestBid = true;
        //                    biddingRoom.HasFinished = true;

        //                    _unitOfWork.BiddingRooms.Update(biddingRoom);
        //                    _unitOfWork.Bids.Update(winningBid);
        //                    await _unitOfWork.SaveChangesAsync();

        //                    _logger.LogInformation($"Winning bid updated for bidding room {roomId}. Winning bid: {winningBid.Id}");
        //                }
        //                else
        //                {
        //                    _logger.LogError("No winning bid found for the specified room.");
        //                }
        //            }
        //            else
        //            {
        //                _logger.LogError("No bids found for the specified room.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error occurred while updating winning bid: {ex}");
        //    }
        //}
    }
}
