using System.ComponentModel.DataAnnotations;

namespace HeightsAuction.Application.DTOs
{
    public class CreateRoomRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
    }
}
