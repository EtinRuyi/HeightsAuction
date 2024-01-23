using HeightsAuction.Domain.Entities.SharedEntities;

namespace HeightsAuction.Domain.Entities
{
    public class Bid : BaseEntity
    {
        public decimal Amount { get; set; }
        public string BiddingRoomId { get; set; }
        public BiddingRoom BiddingRoom { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime BidTime { get; set; }
    }
}
