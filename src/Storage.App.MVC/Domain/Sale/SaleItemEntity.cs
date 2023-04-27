using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Domain.Core;

namespace Storage.App.MVC.Core.Sale
{
    public class SaleItemEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }

        public ProductEntity Product { get; set; }
        public Guid ProductId { get; set; }
        public SaleEntity Sale { get; set; }
        public Guid SaleId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public Guid EnterpriseId { get; set; }
    }
}
