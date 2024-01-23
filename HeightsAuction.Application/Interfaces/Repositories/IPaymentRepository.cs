using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetInvoiceByIdAsync(string id);
        Task<List<Payment>> GetPaymentByBiddingRoomIdAsync(Expression<Func<Invoice, bool>> condition);
        Task CreatePaymentAsync(Payment payment);
        void UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Payment payment);
        Task<List<Payment>> FindBiddingRooms(Expression<Func<Payment, bool>> expression);
    }
}
