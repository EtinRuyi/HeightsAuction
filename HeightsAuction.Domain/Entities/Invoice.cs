using HeightsAuction.Domain.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeightsAuction.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        [ForeignKey("BiddingRoomId")]
        public string BiddingRoomId { get; set; }
        //public BiddingRoom BiddingRoom { get; set; }
        [ForeignKey("AppUserId")]
        public string UserId { get; set; }
        //public AppUser AppUser { get; set; }
        [ForeignKey("BidId")]
        public string WinningBidId { get; set; }
        //public Bid WinningBid { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
