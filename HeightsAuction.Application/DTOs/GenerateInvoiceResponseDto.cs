namespace HeightsAuction.Application.DTOs
{
    public class GenerateInvoiceResponseDto
    {
        public string InvoiceId { get; set; }
        public string BiddingRoomId { get; set; }
        public string UserId { get; set; }
        public string WinningBidId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
