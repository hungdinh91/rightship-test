namespace OrderSystem.Shared.Contracts;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task UpdateManyAsync(IEnumerable<T> entities);
    Task DeleteAsync(Guid id);
}
