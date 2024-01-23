using HeightsAuction.Domain.Entities.SharedEntities;
using HeightsAuction.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeightsAuction.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        [ForeignKey("AppUserId")]
        public string UserId { get; set; }
    }
}
