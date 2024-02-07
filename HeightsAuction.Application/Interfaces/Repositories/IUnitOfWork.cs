namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IBiddingRoomRepository BiddingRooms { get; }
        IBidRepository Bids { get; }
        IInvoiceRepository Invoices { get; }
        IPaymentRepository Payments { get; }
        IBidNotification Notifications { get; }
        IUserRepository Users { get; }
        IItemRepository Items { get; }
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
