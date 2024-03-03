using HeightsAuction.Application.DTOs;
using HeightsAuction.Domain;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<ApiResponse<GenerateInvoiceResponseDto>> GenerateInvoiceAsync(string roomId);
    }
}
