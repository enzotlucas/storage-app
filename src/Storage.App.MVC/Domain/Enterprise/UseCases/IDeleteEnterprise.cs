using Storage.App.MVC.Domain.Core;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Domain.Enterprise.UseCases
{
    public interface IDeleteEnterprise
    {
        Task<BaseResult> RunAsync(Guid id, Guid enterpriseId, CancellationToken cancellationToken);
    }
}
