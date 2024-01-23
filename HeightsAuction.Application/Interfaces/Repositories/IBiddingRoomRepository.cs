using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBiddingRoomRepository : IGenericRepository<BiddingRoom>
    {
        Task<BiddingRoom> GetBiddingRoomByIdAsync(string id);
        Task<List<BiddingRoom>> GetAllBiddingRoomssAsync();
        Task<List<BiddingRoom>> FindBiddingRooms(Expression<Func<BiddingRoom, bool>> expression);
        Task CreateBiddingRoomAsync(BiddingRoom biddingRoom);
        void UpdateBiddingRoomAsync(BiddingRoom biddingRoom);
        Task DeleteBiddingRoomAsync(BiddingRoom biddingRoom);
        Task<List<BiddingRoom>> ActiveAuctionsAsync();
        Task<BiddingRoom> GetBiddingRoomWinnerAsync(Expression<Func<BiddingRoom, bool>> expression);
    }
}
