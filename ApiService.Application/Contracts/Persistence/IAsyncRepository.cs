namespace ApiService.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
}
