using Storage.App.MVC.Core.ActivityHistory;

namespace Storage.App.MVC.Models
{
    public class ActivityHistoryViewModel
    {
        public Guid Id { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
