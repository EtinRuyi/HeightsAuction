using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;

namespace HeightsAuction.Persistence.Repositories
{

    public class BidNotificationRepository : GenericRepository<BidNotification>, IBidNotification
    {
        public BidNotificationRepository(HAuctionDBContext context) : base(context) { }
        public async Task CreateNotificationAsync(BidNotification notification) => await AddAsync(notification);
    }
}
