using Storage.App.MVC.Models;

namespace Storage.App.MVC.Core.ActivityHistory.UseCases
{
    public interface IGetActivity
    {
        Task<IEnumerable<ActivityHistoryViewModel>> RunAsync(Guid enterpriseId,
                                                             ActivityType activityType,
                                                             CancellationToken cancellationToken);
    }
}
