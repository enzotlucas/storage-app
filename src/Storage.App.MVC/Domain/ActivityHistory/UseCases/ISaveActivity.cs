namespace Storage.App.MVC.Core.ActivityHistory.UseCases
{
    public interface ISaveActivity
    {
        Task RunAsync(Guid enterpriseId,
                      Guid objectId,
                      ActivityType activityType,
                      string description,
                      CancellationToken cancellationToken);
    }
}
