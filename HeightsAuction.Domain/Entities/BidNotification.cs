using HeightsAuction.Domain.Entities.SharedEntities;

namespace HeightsAuction.Domain.Entities
{
    public class BidNotification : BaseEntity
    {
        public string Message { get; set; }
        public decimal CurrentBid { get; set; }
        public string BiddingRoomId { get; set; }
        public BiddingRoom BiddingRoom { get; set; }
        public string BidderId { get; set; }
        public AppUser Bidder { get; set; }
        public DateTime NotificationTime { get; set; }
    }
}
