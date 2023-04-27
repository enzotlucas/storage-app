using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Domain.ActivityHistory;

namespace Storage.App.MVC.Infrastructure.Database.Mappings
{
    public sealed class ActivityHistoryMapping : IEntityTypeConfiguration<ActivityHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityHistoryEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Description)
                 .IsRequired()
                 .HasMaxLength(500)
                 .IsRequired();

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ActivityType)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasConversion(activityType => Enum.GetName(activityType),
                                  activityTypeName => (ActivityType)Enum.Parse(typeof(ActivityType), activityTypeName));

            builder.Property(a => a.ActivityAction)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasConversion(activityAction => Enum.GetName(activityAction),
                                  activityActionName => (ActivityAction)Enum.Parse(typeof(ActivityAction), activityActionName));

            builder.HasOne(a => a.Enterprise)
                   .WithMany(e => e.ActivityHistory)
                   .HasForeignKey(s => s.EnterpriseId);
        }
    }
}
