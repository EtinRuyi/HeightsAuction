namespace HeightsAuction.Application.DTOs
{
    public class CreateRoomRequestDto
    {
        public string Title { get; set; }
        public string ItemId { get; set; }
        public DateTime? AuctionStartDate { get; set; }
        public DateTime? AuctionEndDate { get; set; }
    }
}
