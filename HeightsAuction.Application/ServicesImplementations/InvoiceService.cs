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
        private readonly IBiddingService _biddingService;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, 
            ILogger<InvoiceService> logger, IBiddingService biddingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _biddingService = biddingService;
        }

        public async Task<ApiResponse<GenerateInvoiceResponseDto>> GenerateInvoiceAsync(string roomId)
        {
            try
            {
                var winningBidApiResponse = await _biddingService.GetWinningBidFromRoom(roomId);
                if (winningBidApiResponse.StatusCode != 200)
                    return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, winningBidApiResponse.Message, winningBidApiResponse.StatusCode, new List<string>());

                var winningBidDto = winningBidApiResponse.Data;

                var winningBid = await _unitOfWork.Bids.GetByIdAsync(winningBidDto.BidId);
                if (winningBid == null || !winningBid.IsHeighestBid)
                    return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, "Invalid or non-winning bid provided", 400, new List<string>());

                var item = await _unitOfWork.Items.GetByIdAsync(winningBid.ItemId);
                if (item == null)
                    return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, "Associated item not found", 404, new List<string>());

                var invoice = new Invoice
                {
                    BiddingRoomId = roomId,
                    UserId = winningBid.UserId,
                    WinningBidId = winningBidDto.BidId
                };
                await _unitOfWork.Invoices.GenerateInvoiceAsync(invoice);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<GenerateInvoiceResponseDto>(invoice);
                responseDto.ItemId = winningBid.ItemId;
                responseDto.Amount = winningBid.Amount;
                responseDto.BidTime = winningBid.BidTime;

                return ApiResponse<GenerateInvoiceResponseDto>.Success(responseDto, "Invoice generated successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while generating invoice: {ex}");
                return ApiResponse<GenerateInvoiceResponseDto>.Failed(false, "Error occurred while generating invoice", 500, new List<string>());
            }
        }
    }
}