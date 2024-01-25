using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<List<AppUser>> GetUsersAsync();
    }
}
