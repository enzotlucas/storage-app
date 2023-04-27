using Storage.App.MVC.Domain.ActivityHistory;

namespace Storage.App.MVC.Core.ActivityHistory.UseCases
{
    public interface ISaveActivity
    {
        Task RunAsync(Guid enterpriseId,
                      Guid objectId,
                      ActivityType activityType,
                      ActivityAction activityAction,
                      string description,
                      CancellationToken cancellationToken);
    }
}
