namespace HeightsAuction.Application.DTOs
{
    public class BidResponseDto
    {
        public string BidId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
        public string UserId { get; set; }
        public string BiddingRoomId { get; set; }
        public string ItemId { get; set; }
        public bool IsHeighestBid { get; set; }
    }
}
