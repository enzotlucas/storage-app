using Storage.App.MVC.Models.Enterprise;

namespace Storage.App.MVC.Domain.Customer.UseCases
{
    public interface IGetEnterpriseById
    {
        Task<EnterpriseViewModel> RunAsync(Guid id, CancellationToken cancellationToken);
    }
}
