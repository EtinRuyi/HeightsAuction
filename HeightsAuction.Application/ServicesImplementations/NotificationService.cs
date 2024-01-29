using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Domain;
using HeightsAuction.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HeightsAuction.Application.ServicesImplementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<BidNotificationResponseDto>> NotifyParticipantsAsync(BidNotificationRequestDto requestDto)
        {
            try
            {
                var notification = new BidNotification
                {
                    Message = requestDto.Message,
                    CurrentBid = requestDto.CurrentBid,
                    BiddingRoomId = requestDto.BiddingRoomId,
                    BidderId = requestDto.BidderId,
                    NotificationTime = DateTime.UtcNow
                };

                await _unitOfWork.Notifications.CreateNotificationAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<BidNotificationResponseDto>(notification);

                return ApiResponse<BidNotificationResponseDto>.Success(responseDto, "Participants notified successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while notifying participants: {ex}");
                return ApiResponse<BidNotificationResponseDto>.Failed(false, "Error occurred while notifying participants", 500, new List<string>());
            }
        }

        public async Task<ApiResponse<BidNotificationResponseDto>> NotifyAuctionConclusionAsync(BidNotificationRequestDto requestDto)
        {
            try
            {
                var notification = new BidNotification
                {
                    Message = "Auction has concluded. Winner: " + requestDto.BidderId,
                    BiddingRoomId = requestDto.BiddingRoomId,
                    NotificationTime = DateTime.UtcNow
                };

                await _unitOfWork.Notifications.CreateNotificationAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<BidNotificationResponseDto>(notification);

                return ApiResponse<BidNotificationResponseDto>.Success(responseDto, "Auction conclusion notified successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while notifying auction conclusion: {ex}");
                return ApiResponse<BidNotificationResponseDto>.Failed(false, "Error occurred while notifying auction conclusion", 500, new List<string>());
            }
        }
    }
}
