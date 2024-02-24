using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Domain;
using HeightsAuction.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HeightsAuction.Application.ServicesImplementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InvoiceService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<GenerateInvoiceResponseDto>> GenerateInvoiceAsync(GenerateInvoiceRequestDto requestDto)
        {
            try
            {
                var biddingRoom = await _unitOfWork.BiddingRooms.GetRoomByIdAsync(requestDto.BiddingRoomId);
                if (biddingRoom == null)
                {
                    return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, "Bidding room not found", 404, new List<string>());
                }

                if (biddingRoom.AuctionEndDate <= DateTime.UtcNow)
                {
                    biddingRoom.HasFinished = true;
                    return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, "Bidding room has finished", 400, new List<string>());
                }

                var winningBid = biddingRoom.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();

                if (winningBid == null)
                {
                    return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, "No winning bid found for the specified room", 404, new List<string>());
                }

                var invoice = new Invoice
                {
                    BiddingRoomId = requestDto.BiddingRoomId,
                    UserId = requestDto.UserId,
                    WinningBidId = winningBid.Id
                };
                await _unitOfWork.Invoices.GenerateInvoiceAsync(invoice);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<GenerateInvoiceResponseDto>(invoice);

                return ApiResponse<GenerateInvoiceResponseDto>.Success(responseDto, "Invoice generated successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while generating an invoice: {ex}");
                return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, "Error occurred while generating an invoice", 500, new List<string>());
            }
        }
    }
}