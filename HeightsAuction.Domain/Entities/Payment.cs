using HeightsAuction.Domain.Entities.SharedEntities;
using HeightsAuction.Domain.Enums;

namespace HeightsAuction.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
