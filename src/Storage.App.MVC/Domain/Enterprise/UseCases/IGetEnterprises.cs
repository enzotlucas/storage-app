using Storage.App.MVC.Models.Enterprise;

namespace Storage.App.MVC.Domain.Enterprise.UseCases
{
    public interface IGetEnterprises
    {
        Task<IEnumerable<EnterpriseViewModel>> RunAsync(CancellationToken cancellationToken);
    }
}
