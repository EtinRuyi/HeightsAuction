using HeightsAuction.Domain.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeightsAuction.Domain.Entities
{
    public class Bid : BaseEntity
    {
        public decimal Amount { get; set; }
        [ForeignKey("ItemId")]
        public string ItemId { get; set; } 
        [ForeignKey("BiddingRoomId")]
        public string BiddingRoomId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public bool IsHeighestBid { get; set; }
        public DateTime BidTime { get; set; }
    }
}
