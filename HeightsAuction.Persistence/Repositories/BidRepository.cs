using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;
using System.Linq.Expressions;

namespace HeightsAuction.Persistence.Repositories
{
    public class BidRepository : GenericRepository<Bid>, IBidRepository
    {
        public BidRepository(HAuctionDBContext context) : base(context) { }

        public async Task AddBidAsync(Bid bid) => await AddAsync(bid);

        public async Task<List<Bid>> FindBids(Expression<Func<Bid, bool>> expression) => await FindAsync(expression);

        public async Task<List<Bid>> GetAllItemsAsync() => await GetAllAsync();

        public Task GetWinningBidByRoomId(Expression<Func<Bid, bool>> condition) => FindAsync(condition);
    }
}
