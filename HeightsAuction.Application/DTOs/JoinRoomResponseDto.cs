using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.DTOs
{
    public class JoinRoomResponseDto
    {
        public string RoomId { get; set; }
        public string Title { get; set; }
        public Item Item { get; set; }
        public string UserId { get; set; }
        public ICollection<AppUser> Bidders { get; set; } = new List<AppUser>();
        public DateTime? AuctionStartDate { get; set; }
        public DateTime? AuctionEndDate { get; set; }
        public bool HasFinished { get; set; }
    }
}
