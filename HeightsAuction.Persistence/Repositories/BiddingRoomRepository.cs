﻿using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Persistence.AppContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HeightsAuction.Persistence.Repositories
{
    public class BiddingRoomRepository : GenericRepository<BiddingRoom>, IBiddingRoomRepository
    {
        public BiddingRoomRepository(HAuctionDBContext context) : base(context) { }

        public async Task CreateRoomAsync(BiddingRoom biddingRoom) => await AddAsync(biddingRoom);

        public async Task<List<BiddingRoom>> FindRooms(Expression<Func<BiddingRoom, bool>> expression) => await FindAsync(expression);

        public async Task<List<BiddingRoom>> GetAllRoomssAsync() => await GetAllAsync();

        public async Task<BiddingRoom> GetRoomByIdAsync(string roomId)
        {
            return await _context.BiddingRooms
            .Include(b => b.Item)
            .Include(b => b.Bids)
            .Include(b => b.Bidders)
            .FirstOrDefaultAsync(b => b.Id == roomId);
        }

        public async Task<BiddingRoom> IncludeRelatedEntities(BiddingRoom biddingRoom)
        {
            return await _context.BiddingRooms
                .Include(b => b.Item)
                .Include(b => b.Bids)
                .Include(b => b.Bidders)
                .FirstOrDefaultAsync(b => b.Id == biddingRoom.Id);
        }

    }
}
