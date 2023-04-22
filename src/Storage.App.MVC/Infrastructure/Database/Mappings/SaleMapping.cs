using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Infrastructure.Database.Mappings
{
    public class SaleMapping : IEntityTypeConfiguration<SaleEntity>
    {
        public void Configure(EntityTypeBuilder<SaleEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Note)
                 .IsRequired()
                 .HasMaxLength(500);

            builder.Property(s => s.TotalPrice).IsRequired();

            builder.HasMany(s => s.SaleItems)
                   .WithOne(si => si.Sale)
                   .HasForeignKey(si => si.SaleId);

            builder.HasOne(s => s.Customer)
                   .WithMany(c => c.Sales);

            builder.HasOne(s => s.Enterprise)
                   .WithMany(e => e.Sales)
                   .HasForeignKey(s => s.EnterpriseId);
        }
    }
}
