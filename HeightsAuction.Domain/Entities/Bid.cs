using HeightsAuction.Domain.Entities.SharedEntities;

namespace HeightsAuction.Domain.Entities
{
    public class Bid : BaseEntity
    {
        public decimal Amount { get; set; }
        public string ItemId { get; set; } 
        public Item Item { get; set; }
        public string BiddingRoomId { get; set; }
        public BiddingRoom BiddingRoom { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<AppUser> UserBids { get; set; } = new List<AppUser>();
        public bool IsHeighestBid { get; set; }
        public DateTime BidTime { get; set; }
    }
}
