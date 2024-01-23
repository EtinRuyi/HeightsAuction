using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBidNotification : IGenericRepository<BidNotification>
    {
        Task<List<BidNotification>> GetNotificationsAsync();
        Task CreateNotificationAsync(BidNotification notification);
        void UpdateNotificationAsync(BidNotification notification);
        Task DeleteNotificationAsync(BidNotification notificationId);
    }
}
