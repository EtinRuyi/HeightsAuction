namespace HeightsAuction.Application.DTOs
{
    public class AddBidResponseDto
    {
        public string BidId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
        public string UserId { get; set; }
        public string BiddingRoomId { get; set; }
    }
}
