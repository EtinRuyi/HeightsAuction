using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;
using Microsoft.EntityFrameworkCore;

namespace HeightsAuction.Persistence.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(HAuctionDBContext context) : base(context) { }

        public async Task CreateItemAsync(Item item) => await AddAsync(item);

        public async Task<List<Item>> GetAllItemsAsync() => await GetAllAsync();

        public async Task<Item> IncludeRelatedEntities(Item item)
        {
            return await _context.Items
                .Include(b => b.Bids)
                .FirstOrDefaultAsync(b => b.Id == item.Id);
        }
    }
}
