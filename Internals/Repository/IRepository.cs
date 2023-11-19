namespace Internals.Repository;

public interface IRepository<T, in TK>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(TK id);
    Task<T> AddAsync(T obj);
    Task<T> UpdateAsync(T obj);
    Task DeleteAsync(TK id);
}