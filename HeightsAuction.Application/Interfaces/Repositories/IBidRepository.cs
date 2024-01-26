using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task<List<Bid>> GetAllBidsAsync();
        //Task<List<Bid>> GetBidByBiddingRoomIdAsync();
        Task AddBidAsync(Bid bid);
        Task DeleteBidAsync(Bid bid);
        void UpdateBidAsync(Bid bid);
        Task<List<Bid>> FindBids(Expression<Func<Bid, bool>> expression);
    }
}
