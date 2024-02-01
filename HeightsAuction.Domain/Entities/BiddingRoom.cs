using HeightsAuction.Domain.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeightsAuction.Domain.Entities
{
    public class BiddingRoom : BaseEntity
    {
        public string Title { get; set; }
        [ForeignKey("ItemId")]
        public string ItemId { get; set; }
        public Item Item { get; set; }
        [ForeignKey("BidId")]
        public string WinningBidId { get; set; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public ICollection<AppUser> Bidders { get; set; } = new List<AppUser>();
        public DateTime? AuctionStartDate { get; set; }
        public DateTime? AuctionEndDate { get; set; }
        public bool HasFinished { get; set; }
    }
}
