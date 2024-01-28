using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task AddBidAsync(Bid bid);
        Task GetWinningBidByRoomId(Expression<Func<Bid, bool>> condition);
        Task<List<Bid>> FindBids(Expression<Func<Bid, bool>> expression);
    }
}
