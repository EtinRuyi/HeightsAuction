using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Persistence.AppContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HeightsAuction.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly HAuctionDBContext _context;

        public GenericRepository(HAuctionDBContext context) =>_context = context;

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void DeleteAsync(T entity) => _context.Set<T>().Remove(entity);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression) => await _context.Set<T>().Where(expression).ToListAsync();

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().AnyAsync(predicate);

        public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(string id) => await _context.Set<T>().FindAsync(id);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Update(T entity) => _context.Set<T>().Update(entity);
    }
}
