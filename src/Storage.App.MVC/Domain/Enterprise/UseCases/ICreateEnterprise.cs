using Storage.App.MVC.Domain.Core;
using Storage.App.MVC.Models.Enterprise;

namespace Storage.App.MVC.Domain.Enterprise.UseCases
{
    public interface ICreateEnterprise
    {
        Task<BaseResult> RunAsync(EnterpriseViewModel enterpriseViewModel, Guid enterpriseId, CancellationToken cancellationToken);
    }
}
