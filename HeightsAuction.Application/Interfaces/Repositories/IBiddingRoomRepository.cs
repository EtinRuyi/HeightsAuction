using HeightsAuction.Domain.Entities;
using System.Linq.Expressions;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IBiddingRoomRepository : IGenericRepository<BiddingRoom>
    {
        Task<BiddingRoom> GetRoomByIdAsync(string roomId);
        Task<List<BiddingRoom>> GetAllRoomssAsync();
        Task<List<BiddingRoom>> FindRooms(Expression<Func<BiddingRoom, bool>> expression);
        Task CreateRoomAsync(BiddingRoom biddingRoom);
        Task<BiddingRoom> IncludeRelatedEntities(BiddingRoom biddingRoom);
    }
}
