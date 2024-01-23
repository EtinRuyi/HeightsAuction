using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HeightsAuction.Persistence.Repositories
{
    public class BiddingRoomRepository : GenericRepository<BiddingRoom>, IBiddingRoomRepository
    {
        public BiddingRoomRepository(HAuctionDBContext context) : base(context) { }

        public Task<List<BiddingRoom>> ActiveAuctionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateBiddingRoomAsync(BiddingRoom biddingRoom)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBiddingRoomAsync(BiddingRoom biddingRoom)
        {
            throw new NotImplementedException();
        }

        public Task<List<BiddingRoom>> FindBiddingRooms(Expression<Func<BiddingRoom, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<List<BiddingRoom>> GetAllBiddingRoomssAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BiddingRoom> GetBiddingRoomByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BiddingRoom> GetBiddingRoomWinnerAsync(Expression<Func<BiddingRoom, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void UpdateBiddingRoomAsync(BiddingRoom biddingRoom) => Update(biddingRoom);
    }
}
