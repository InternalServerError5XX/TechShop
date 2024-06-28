using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.BaseService
{
    public class BaseService<T>(IBaseRepository<T> baseRepository) : IBaseService<T> where T : class
    {
        public IQueryable<T> GetAllAsync()
        {
            return baseRepository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await baseRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await baseRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await baseRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await baseRepository.DeleteAsync(id);
        }
    }
}
