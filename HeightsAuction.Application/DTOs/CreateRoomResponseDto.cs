namespace HeightsAuction.Application.DTOs
{
    public class CreateRoomResponseDto
    {
        public string RoomId { get; set; }
        public string Title { get; set; }
        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
        public string CreatedBy { get; set; }
        public bool HasFinished { get; set; }
    }
}
