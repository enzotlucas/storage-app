using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Core.Enterprise
{
    public class EnterpriseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<CustomerEntity> Customers { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public ICollection<SaleEntity> Sales { get; set; }
        public ICollection<SaleItemEntity> SaleItems { get; set; }
        public ICollection<ActivityHistoryEntity> ActivityHistory { get; set; }
    }
}
