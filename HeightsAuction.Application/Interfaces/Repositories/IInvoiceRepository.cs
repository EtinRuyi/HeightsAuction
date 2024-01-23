using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> GetInvoiceByIdAsync(string id);
        Task<List<Invoice>> GetInvoiceByBiddingRoomIdAsync(Expression<Func<Invoice, bool>> condition);
        Task GenerateInvoiceAsync(Invoice invoice);
        void UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(Invoice invoice);
        Task<List<Invoice>> FindInvoices(Expression<Func<Invoice, bool>> expression);
    }
}
