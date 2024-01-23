using HeightsAuction.Domain.Entities.SharedEntities;

namespace HeightsAuction.Domain.Entities
{
    public class BiddingRoom : BaseEntity
    {
        public string RoomName { get; set; }
        public string ItemName { get; set; }
        public string WinningBidId { get; set; }
        public Bid WinningBid { get; set; }
        public List<Bid> Bids { get; set; } = new List<Bid>();
        public ICollection<AppUser> Participants { get; set; } = new List<AppUser>();
        public DateTime? EndTime { get; set; }
        public bool IsRoomActive { get; set; }
    }
}
