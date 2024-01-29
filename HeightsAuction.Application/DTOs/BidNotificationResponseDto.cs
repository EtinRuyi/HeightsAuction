namespace HeightsAuction.Application.DTOs
{
    public class BidNotificationResponseDto
    {
        public string NotificationId { get; set; }
        public string Message { get; set; }
        public decimal CurrentBid { get; set; }
        public string BiddingRoomId { get; set; }
        public string BidderId { get; set; }
        public DateTime NotificationTime { get; set; }
    }
}
