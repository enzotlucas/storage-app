using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Product;

namespace Storage.App.MVC.Core.Sale
{
    public class SaleEntity
    {
        public Guid Id { get; set; }        
        public string Note { get; set; }       
        public decimal TotalPrice { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<SaleItemEntity> SaleItems { get; set; }
    }
}
