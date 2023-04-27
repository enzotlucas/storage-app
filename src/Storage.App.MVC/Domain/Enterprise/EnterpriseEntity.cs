using Microsoft.AspNetCore.Identity;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Core.Enterprise
{
    public class EnterpriseEntity : IdentityUser
    {
        public new Guid Id
        {
            get
            {
                return Guid.Parse(base.Id);
            }
            set
            {
                base.Id = value.ToString();
            }
        }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<CustomerEntity> Customers { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public ICollection<SaleEntity> Sales { get; set; }
        public ICollection<SaleItemEntity> SaleItems { get; set; }
        public ICollection<ActivityHistoryEntity> ActivityHistory { get; set; }

        public bool Exists()
        {
            return Id != Guid.Empty;
        }
    }
}
