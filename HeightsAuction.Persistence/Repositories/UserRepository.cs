using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;

namespace HeightsAuction.Persistence.Repositories
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(HAuctionDBContext context) : base(context) { }

        public async Task<List<AppUser>> GetUsersAsync()
        {
            return await GetAllAsync();
        }
    }
}
