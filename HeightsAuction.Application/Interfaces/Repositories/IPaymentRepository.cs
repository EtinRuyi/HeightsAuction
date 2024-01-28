using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task CreatePaymentAsync(Payment payment);
    }
}
