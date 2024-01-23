using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;
using System.Net.Sockets;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task<Bid> GetBidByIdAsync(string id);
        Task<List<Bid>> GetAllBidsAsync();
        Task<List<Bid>> GetBidByBiddingRoomIdAsync(Expression<Func<Bid, bool>> condition);
        Task PlaceBidAsync(Bid bid);
        Task DeleteKycAsync(Bid bid);
        void UpdateBidAsync(Bid bid);
        Task<List<Bid>> FindBids(Expression<Func<Bid, bool>> expression);
    }
}
