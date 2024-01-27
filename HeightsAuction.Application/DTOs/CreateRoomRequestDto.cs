namespace HeightsAuction.Application.DTOs
{
    public class CreateRoomRequestDto
    {
        public string Title { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemStartingPrice { get; set; }
        public DateTime? AuctionStartDate { get; set; }
        public DateTime? AuctionEndDate { get; set; }
    }
}
