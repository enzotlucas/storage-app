using Storage.App.MVC.Models;

namespace Storage.App.MVC.Core.ActivityHistory.UseCases
{
    public interface IGetActivityById
    {
        Task<ActivityHistoryViewModel> RunAsync(Guid id, CancellationToken cancellationToken);
    }
}
