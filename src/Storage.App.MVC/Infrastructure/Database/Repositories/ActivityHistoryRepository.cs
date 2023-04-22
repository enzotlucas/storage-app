using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public class ActivityHistoryRepository : IActivityHistoryRepository
    {
        private readonly SqlServerContext _context;

        public ActivityHistoryRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ActivityHistoryEntity>> GetAllAsync()
        {
            var activityHistory = await _context.ActivityHistory.Include(s => s.Enterprise)
                                                      .ToListAsync();

            return activityHistory.Count > 0 ? activityHistory : Enumerable.Empty<ActivityHistoryEntity>();
        }

        public async Task<ActivityHistoryEntity> GetByIdAsync(Guid id)
        {
            var activityHistory = await _context.ActivityHistory.Include(s => s.Enterprise)
                                                     .FirstOrDefaultAsync(s => s.Id == id);

            return activityHistory ?? new ActivityHistoryEntity();        
        }

        public async Task<ActivityHistoryEntity> CreateAsync(ActivityHistoryEntity activityHistory)
        {
            activityHistory.Id = Guid.NewGuid();

            await _context.AddAsync(activityHistory);

            return activityHistory;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
