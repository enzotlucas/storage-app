using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Models.Enterprise
{
    public class EnterprisesPageViewModel
    {
        public List<EnterpriseViewModel> Enterprises { get; set; }
        public List<ActivityHistoryViewModel> ActivityHistory { get; set; }
    }
}
