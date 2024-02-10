using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.DTOs
{
    public class ItemResponseDto
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CurrentBidPrice { get; set; }
        public string BiddingRoomId { get; set; }
        public string UserId { get; set; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
