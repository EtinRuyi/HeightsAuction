using HeightsAuction.Domain.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeightsAuction.Domain.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CurrentBidPrice { get; set; }
        [Required]
        [ForeignKey("BiddingRoomId")]
        public string BiddingRoomId { get; set; }
        public BiddingRoom BiddingRoom { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
