namespace Storage.App.MVC.Core.Domain
{
    public interface IBaseRepository<T> : IDisposable 
    {
        Task<IEnumerable<T>> GetAllAsync(Guid enterpriseId, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
