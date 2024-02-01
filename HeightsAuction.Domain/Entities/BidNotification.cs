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
        //public BiddingRoom BiddingRoom { get; set; }
        [ForeignKey("AppUserId")]
        public string BidderId { get; set; }
        //public AppUser Bidder { get; set; }
        public DateTime NotificationTime { get; set; }
    }
}
