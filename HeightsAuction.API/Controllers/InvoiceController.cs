using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateInvoice(string roomId)
        {
            var result = await _invoiceService.GenerateInvoiceAsync(roomId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
