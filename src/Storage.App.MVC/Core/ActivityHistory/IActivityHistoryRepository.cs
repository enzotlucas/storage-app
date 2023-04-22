namespace Storage.App.MVC.Core.ActivityHistory
{
    public interface IActivityHistoryRepository : IDisposable
    {
        Task<IEnumerable<ActivityHistoryEntity>> GetAllAsync();
        Task<ActivityHistoryEntity> GetByIdAsync(Guid id);
        Task<ActivityHistoryEntity> CreateAsync(ActivityHistoryEntity activityHistory);
    }
}
