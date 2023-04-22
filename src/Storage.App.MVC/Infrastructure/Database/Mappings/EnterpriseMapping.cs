using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Enterprise;

namespace Storage.App.MVC.Infrastructure.Database.Mappings
{
    public class EnterpriseMapping : IEntityTypeConfiguration<EnterpriseEntity>
    {
        public void Configure(EntityTypeBuilder<EnterpriseEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                 .IsRequired()
                 .HasMaxLength(200);

            builder.HasMany(e => e.Products)
                   .WithOne(p => p.Enterprise)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Customers)
                   .WithOne(c => c.Enterprise)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Sales)
                  .WithOne(c => c.Enterprise)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.SaleItems)
                   .WithOne(c => c.Enterprise)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.ActivityHistory)
                   .WithOne(c => c.Enterprise);
        }
    }
}
