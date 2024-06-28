using Microsoft.EntityFrameworkCore;
using TechShop.Domain.Entities;

namespace TechShop.Infrastructure.Repositories.BaseRepository
{
    public class BaseRepository<T>(DbContext context) : IBaseRepository<T> where T : BaseEntity 
    {
        private readonly DbSet<T> dbSet = context.Set<T>();

        public IQueryable<T> GetAllAsync()
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

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
