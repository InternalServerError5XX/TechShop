using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using TechShop.Domain.Entities;

namespace TechShop.Infrastructure.Repositories.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity 
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private readonly DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet
                .AsQueryable()
                .AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet
                .AsQueryable()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = dbSet
                .AsQueryable()
                .AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> AddAsync(T entity)
        {
            var date = DateTime.Now;
            entity.CreatedDate = date;
            entity.UpdatedDate = date;

            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var date = DateTime.Now;
                entity.CreatedDate = date;
                entity.UpdatedDate = date;
            }

            await dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedDate = DateTime.Now;

            dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
            }
        }

        public async Task RollbackTransactionAsync(Exception ex)
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            catch
            {
                await _transaction.DisposeAsync();
                throw ex;
            }
        }
    }
}
