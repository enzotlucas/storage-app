using Storage.App.MVC.Core.Product;

namespace Storage.App.MVC.Models
{
    public class ProductsPageViewModel
    {
        public List<ProductEntity> Products { get; set; }
        public List<ActivityHistoryViewModel> ActivityHistory { get; set; }
    }
}
