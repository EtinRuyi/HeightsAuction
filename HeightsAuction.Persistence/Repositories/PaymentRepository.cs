using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;

namespace HeightsAuction.Persistence.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(HAuctionDBContext context) : base(context) { }

        public async Task CreatePaymentAsync(Payment payment) => await AddAsync(payment);
    }
}
