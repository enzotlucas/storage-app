using Storage.App.MVC.Core.Customer;

namespace Storage.App.MVC.Models
{
    public class CustomersPageViewModel
    {
        public List<CustomerEntity> Customers { get; set; }
        public List<ActivityHistoryViewModel> ActivityHistory { get; set; }
    }
}
