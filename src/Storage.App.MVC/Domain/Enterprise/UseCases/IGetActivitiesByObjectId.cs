using Storage.App.MVC.Models;

namespace Storage.App.MVC.Domain.Enterprise.UseCases
{
    public interface IGetActivitiesByObjectId
    {
        Task<IEnumerable<ActivityHistoryViewModel>> RunAsync(Guid objectId,
                                                             CancellationToken cancellationToken);
    }
}
