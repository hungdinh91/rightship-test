namespace OrderSystem.Shared.Contracts;

public interface IUnitOfWork
{
    IRepository<T> GetRepository<T>() where T : class;
    Task SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
