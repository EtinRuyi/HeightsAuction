using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;

namespace HeightsAuction.Persistence.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(HAuctionDBContext context) : base(context) { }

        public async Task GenerateInvoiceAsync(Invoice invoice) => await AddAsync(invoice);
    }
}
