using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechShop.Domain.DTOs.FilterDto;
using TechShop.Domain.DTOs.PaginationDto;
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

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            return await baseRepository.GetByIdAsync(id, includes);
        }

        public async Task<T> AddAsync(T entity)
        {
            return await baseRepository.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity)
        {
            return await baseRepository.AddRangeAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await baseRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await baseRepository.DeleteAsync(id);
        }


        public IQueryable<T> GetFilteredQuery(IQueryable<T> query, RequestFilterDto<T> filterDto)
        {
            if (filterDto.SearchTerm != null)
                query = query.Where(filterDto.SearchTerm);

            if (filterDto.OrderBy != null)
                query = filterDto.IsAsc
                    ? query.OrderBy(filterDto.OrderBy)
                    : query.OrderByDescending(filterDto.OrderBy);

            return query;
        }

        public async Task<ResponsePaginationDto<T>> GetPaginated(IQueryable<T> query, RequestPaginationDto pagination)
        {
            var totalCount = await query.CountAsync();
            var data = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new ResponsePaginationDto<T>
            {
                Data = data,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize),
                HasNextPage = pagination.PageNumber * pagination.PageSize < totalCount,
                HasPreviousPage = pagination.PageNumber > 1
            };
        }


        public async Task BeginTransactionAsync()
        {
            await baseRepository.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await baseRepository.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync(Exception ex)
        {
            await baseRepository.RollbackTransactionAsync(ex);
        }
    }
}
