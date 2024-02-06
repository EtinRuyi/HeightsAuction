using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.DTOs
{
    public class BiddingRoomDto
    {
        public string RoomId { get; set; }
        public string Title { get; set; }
        public string ItemId { get; set; }
        public string WinningBidId { get; set; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public ICollection<AppUser> Bidders { get; set; } = new List<AppUser>();
        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
        public string CreatedBy { get; set; }
        public bool HasFinished { get; set; }
    }
}
