using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;
using Storage.App.MVC.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.App.MVC.Core.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        public ISaleRepository SaleRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IActivityHistoryRepository ActivityHistoryRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
