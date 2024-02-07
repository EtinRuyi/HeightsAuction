namespace HeightsAuction.Application.DTOs
{
    public class CreateItemRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
    }
}
