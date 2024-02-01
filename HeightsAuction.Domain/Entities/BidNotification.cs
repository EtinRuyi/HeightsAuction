using HeightsAuction.Domain.Entities.SharedEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeightsAuction.Domain.Entities
{
    public class BidNotification : BaseEntity
    {
        public string Message { get; set; }
        public decimal CurrentBid { get; set; }
        [ForeignKey("BiddingRoomId")]
        public string BiddingRoomId { get; set; }
        [ForeignKey("AppUserId")]
        public string BidderId { get; set; }
        public DateTime NotificationTime { get; set; }
    }
}
