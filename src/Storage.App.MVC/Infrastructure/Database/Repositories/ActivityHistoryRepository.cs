using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public sealed class ActivityHistoryRepository : IActivityHistoryRepository
    {
        private readonly SqlServerContext _context;

        public ActivityHistoryRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActivityHistoryEntity>> GetByAcitivityTypeAsync(Guid enterpriseId, ActivityType activityType, CancellationToken cancellationToken)
        {
            var activityHistory = await _context.ActivityHistory
                                                      .Where(a => activityType == ActivityType.Enterprise ? 
                                                                (a.ActivityType == activityType) : 
                                                                (a.EnterpriseId == enterpriseId && a.ActivityType == activityType))
                                                      .Include(s => s.Enterprise)
                                                      .ToListAsync(cancellationToken);

            return activityHistory.Count > 0 ? activityHistory : Enumerable.Empty<ActivityHistoryEntity>();
        }

        public async Task<IEnumerable<ActivityHistoryEntity>> GetByObjectIdAsync(Guid objectId, CancellationToken cancellationToken)
        {
            var activityHistory = await _context.ActivityHistory
                                                      .Where(a => a.ObjectId == objectId)
                                                      .Include(s => s.Enterprise)
                                                      .ToListAsync(cancellationToken);

            return activityHistory.Count > 0 ? activityHistory : Enumerable.Empty<ActivityHistoryEntity>();
        }

        public async Task<ActivityHistoryEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var activityHistory = await _context.ActivityHistory.Include(s => s.Enterprise)
                                                     .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            return activityHistory ?? new ActivityHistoryEntity { Id = Guid.Empty };        
        }

        public async Task<ActivityHistoryEntity> CreateAsync(ActivityHistoryEntity activityHistory, CancellationToken cancellationToken)
        {
            activityHistory.CreatedAt = DateTime.Now;
            activityHistory.UpdatedAt = DateTime.Now;

            await _context.AddAsync(activityHistory, cancellationToken);

            return activityHistory;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
