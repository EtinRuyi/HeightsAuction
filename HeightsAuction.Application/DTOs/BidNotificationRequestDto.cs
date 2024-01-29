namespace HeightsAuction.Application.DTOs
{
    public class BidNotificationRequestDto
    {
        public string Message { get; set; }
        public decimal CurrentBid { get; set; }
        public string BiddingRoomId { get; set; }
        public string BidderId { get; set; }
    }
}
