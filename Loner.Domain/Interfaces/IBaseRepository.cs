using Loner.Domain.Common;

namespace Loner.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task<PaginatedResponse<T>> GetPaginatedAsync(int PageNumber, int PageSize);
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    Task Delete(string id);
}