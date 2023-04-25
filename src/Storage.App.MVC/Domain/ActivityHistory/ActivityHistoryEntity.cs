﻿namespace Storage.App.MVC.Core.ActivityHistory
{
    public sealed class ActivityHistoryEntity
    {
        public Guid Id { get; set; }
        public Guid ObjectId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid EnterpriseId { get; set; }
        public Enterprise.EnterpriseEntity Enterprise { get; set; }
    }
}