using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlServerContext _context;
        public ISaleRepository SaleRepository { get; set; }
        public ICustomerRepository CustomerRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IActivityHistoryRepository ActivityHistoryRepository { get; set; }

        public UnitOfWork(SqlServerContext context, 
                          ISaleRepository saleRepository, 
                          ICustomerRepository customerRepository, 
                          IProductRepository productRepository, 
                          IActivityHistoryRepository activityHistoryRepository)
        {
            _context = context;
            SaleRepository = saleRepository;
            CustomerRepository = customerRepository;
            ProductRepository = productRepository;
            ActivityHistoryRepository = activityHistoryRepository;
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
                SaleRepository.Dispose();
                CustomerRepository.Dispose();
                ProductRepository.Dispose();
                ActivityHistoryRepository.Dispose();
            }
    }
        }
}
