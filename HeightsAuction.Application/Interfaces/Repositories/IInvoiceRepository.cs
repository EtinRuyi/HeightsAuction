using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task GenerateInvoiceAsync(Invoice invoice);
    }
}
