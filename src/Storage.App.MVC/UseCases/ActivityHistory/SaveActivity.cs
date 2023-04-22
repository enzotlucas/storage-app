using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Domain;

namespace Storage.App.MVC.UseCases.ActivityHistory
{
    public sealed class SaveActivity : ISaveActivity
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetActivity> _logger;

        public SaveActivity(IUnitOfWork uow, ILogger<GetActivity> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task RunAsync(Guid enterpriseId,
                                   ActivityType activityType,
                                   string description,
                                   CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [SaveActivity.RunAsync]");

            var activityHistory = new ActivityHistoryEntity
            {
                EnterpriseId = enterpriseId,
                ActivityType = activityType,
                Description = description,
            };

            await _uow.ActivityHistoryRepository.CreateAsync(activityHistory, cancellationToken);

            _logger.LogDebug("End - [SaveActivity.RunAsync]");
        }
    }
}
