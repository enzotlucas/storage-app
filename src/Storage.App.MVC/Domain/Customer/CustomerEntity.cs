using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Sale;
using Storage.App.MVC.Domain.Core;

namespace Storage.App.MVC.Core.Customer
{
    public class CustomerEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<SaleEntity> Sales { get; set; }
    }
}
