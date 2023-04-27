using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Domain.ActivityHistory;

namespace Storage.App.MVC.Models
{
    public class ActivityHistoryViewModel
    {
        public Guid Id { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityAction ActivityAction { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool Exists()
        {
            return Id != Guid.Empty;
        }
    }
}
