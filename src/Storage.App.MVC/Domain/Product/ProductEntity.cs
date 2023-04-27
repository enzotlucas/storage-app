using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Sale;
using Storage.App.MVC.Domain.Core;

namespace Storage.App.MVC.Core.Product
{
    public class ProductEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Brand { get; set; }

        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<SaleItemEntity> SaleItems { get; set; }
    }
}
