using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBiddingRoomRepository : IGenericRepository<BiddingRoom>
    {
        Task<BiddingRoom> GetRoomByIdAsync(string id);
        Task<List<BiddingRoom>> GetAllRoomssAsync();
        Task<List<BiddingRoom>> FindRooms(Expression<Func<BiddingRoom, bool>> expression);
        Task CreateRoomAsync(BiddingRoom biddingRoom);
    }
}
