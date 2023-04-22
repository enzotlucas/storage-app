using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Models
{
    public class SalesPageViewModel
    {
        public List<SaleEntity> Sales { get; set; }
        public List<ActivityHistoryViewModel> ActivityHistory { get; set; }
    }
}
