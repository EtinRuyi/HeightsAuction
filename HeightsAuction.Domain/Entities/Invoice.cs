using HeightsAuction.Domain.Entities.SharedEntities;

namespace HeightsAuction.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string BiddingRoomId { get; set; }
        public BiddingRoom BiddingRoom { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public string WinningBidId { get; set; }
        public Bid WinningBid { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
