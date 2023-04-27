using Storage.App.MVC.Domain.ActivityHistory;
using Storage.App.MVC.Domain.Core;

namespace Storage.App.MVC.Core.ActivityHistory
{
    public sealed class ActivityHistoryEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ObjectId { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityAction ActivityAction { get; set; }
        public string Description { get; set; }


        public Guid EnterpriseId { get; set; }
        public Enterprise.EnterpriseEntity Enterprise { get; set; }
    }
}
