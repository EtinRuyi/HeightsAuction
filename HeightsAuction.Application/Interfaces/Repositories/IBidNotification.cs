using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBidNotification : IGenericRepository<BidNotification>
    {
        Task CreateNotificationAsync(BidNotification notification);
    }
}
