using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Models
{
    public class EnterprisesPageViewModel
    {
        public List<EnterpriseEntity> Enterprises { get; set; }
        public List<ActivityHistoryViewModel> ActivityHistory { get; set; }
    }
}
