namespace Storage.App.MVC.Core.Domain
{
    public interface IBaseRepository<T> : IDisposable 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
        Task DeleteAsync(T entity);
    }
}
