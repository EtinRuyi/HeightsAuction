namespace HeightsAuction.Application.DTOs
{
    public class CreateItemResponseDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
    }
}
