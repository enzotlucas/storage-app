namespace Storage.App.MVC.Core.ActivityHistory
{
    public interface IActivityHistoryRepository : IDisposable
    {
        Task<IEnumerable<ActivityHistoryEntity>> GetByAcitivityTypeAsync(Guid enterpriseId, ActivityType activityType, CancellationToken cancellationToken);
        Task<IEnumerable<ActivityHistoryEntity>> GetByObjectIdAsync(Guid objectId, CancellationToken cancellationToken);
        Task<ActivityHistoryEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ActivityHistoryEntity> CreateAsync(ActivityHistoryEntity activityHistory, CancellationToken cancellationToken);
    }
}
