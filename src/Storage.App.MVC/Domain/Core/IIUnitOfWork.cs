using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;
using Storage.App.MVC.Domain.Enterprise;

namespace Storage.App.MVC.Core.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        public IEnterpriseRepository Enterprises { get; }
        public ISaleRepository Sales { get; }
        public ICustomerRepository Customers { get; }
        public IProductRepository Products { get; }
        public IActivityHistoryRepository ActivityHistory { get; }

        Task<bool> SaveChangesAsync();
    }
}
