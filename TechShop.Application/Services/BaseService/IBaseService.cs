using System.Linq.Expressions;
using TechShop.Domain.DTOs.FilterDto;
using TechShop.Domain.DTOs.PaginationDto;

namespace TechShop.Application.Services.BaseService
{
    public interface IBaseService<T> where T : class
    {
        IQueryable<T> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<ResponsePaginationDto<T>> GetPaginated(IQueryable<T> query, RequestPaginationDto pagination);
        IQueryable<T> GetFilteredQuery(IQueryable<T> query, RequestFilterDto<T> filterDto);

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync(Exception ex);
    }
}
