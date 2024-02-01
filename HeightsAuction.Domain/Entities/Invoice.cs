using HeightsAuction.Domain.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeightsAuction.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        [ForeignKey("BiddingRoomId")]
        public string BiddingRoomId { get; set; }
        [ForeignKey("AppUserId")]
        public string UserId { get; set; }
        [ForeignKey("BidId")]
        public string WinningBidId { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
