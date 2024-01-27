namespace HeightsAuction.Application.DTOs
{
    public class CreateRoomResponseDto
    {
        public string RoomId { get; set; }
        public string Title { get; set; }
        public string ItemId { get; set; }
        public DateTime? AuctionStartDate { get; set; }
        public DateTime? AuctionEndDate { get; set; }
        public bool HasFinished { get; set; }
    }
}
