namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBiddingRoomRepository BiddingRooms { get; }
        IBidRepository Bids { get; }
        IInvoiceRepository Invoices { get; }
        IPaymentRepository Payments { get; }
        IBidNotification Notifications { get; }
        Task<int> SaveChangesAsync();
    }
}
