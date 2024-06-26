﻿using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Persistence.AppContext;

namespace HeightsAuction.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HAuctionDBContext _dbContext;

        public UnitOfWork(HAuctionDBContext dbContext)
        {
            _dbContext = dbContext;
            BiddingRooms = new BiddingRoomRepository(_dbContext);
            Bids = new BidRepository(_dbContext);
            Invoices = new InvoiceRepository(_dbContext);
            Payments = new PaymentRepository(_dbContext);
            Users = new UserRepository(_dbContext);
            Notifications = new BidNotificationRepository(_dbContext);
            Items = new ItemRepository(_dbContext);
        }

        public IBiddingRoomRepository BiddingRooms { get; private set; }
        public IBidRepository Bids { get; private set; }
        public IInvoiceRepository Invoices { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IBidNotification Notifications { get; private set; }
        public IUserRepository Users { get; private set; }
        public IItemRepository Items { get; private set; }

        public void Dispose() => _dbContext.Dispose();

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
