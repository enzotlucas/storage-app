namespace Storage.App.MVC.Core.ActivityHistory.UseCases
{
    public interface ISaveActivity
    {
        Task RunAsync(Guid enterpriseId,
                      ActivityType activityType,
                      string description,
                      CancellationToken cancellationToken);
    }
}
