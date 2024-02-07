using HeightsAuction.Domain.Entities;

namespace HeightsAuction.Application.Interfaces.Repositories
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task CreateItemAsync(Item item);
        Task<List<Item>> GetAllItemsAsync();
        Task<Item> IncludeRelatedEntities(Item item);
    }
}
