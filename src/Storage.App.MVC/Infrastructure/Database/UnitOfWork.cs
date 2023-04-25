using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;
using Storage.App.MVC.Domain.Enterprise;

namespace Storage.App.MVC.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlServerContext _context;
        public IEnterpriseRepository Enterprises { get; set; }
        public ISaleRepository Sales { get; set; }
        public ICustomerRepository Customers { get; set; }
        public IProductRepository Products { get; set; }
        public IActivityHistoryRepository ActivityHistory { get; set; }

        public UnitOfWork(SqlServerContext context,
                          IEnterpriseRepository enterprises,
                          ISaleRepository saleRepository,
                          ICustomerRepository customerRepository,
                          IProductRepository productRepository,
                          IActivityHistoryRepository activityHistoryRepository)
        {
            _context = context;
            Enterprises = enterprises;
            Sales = saleRepository;
            Customers = customerRepository;
            Products = productRepository;
            ActivityHistory = activityHistoryRepository;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                Sales.Dispose();
                Customers.Dispose();
                Products.Dispose();
                ActivityHistory.Dispose();
            }
    }
        }
}
