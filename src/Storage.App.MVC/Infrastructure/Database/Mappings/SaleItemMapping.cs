using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Infrastructure.Database.Mappings
{
    public sealed class SaleItemMapping : IEntityTypeConfiguration<SaleItemEntity>
    {
        public void Configure(EntityTypeBuilder<SaleItemEntity> builder)
        {
            builder.HasKey(si => si.Id);

            builder.Property(si => si.Count).IsRequired();

            builder.Property(si => si.TotalPrice).IsRequired();

            builder.HasOne(si => si.Product)
                   .WithMany(p => p.SaleItems)
                   .HasForeignKey(si => si.ProductId);

            builder.HasOne(si => si.Sale)
                   .WithMany(s => s.SaleItems)
                   .HasForeignKey(s => s.SaleId);

            builder.HasOne(si => si.Enterprise)
                   .WithMany(s => s.SaleItems)
                   .HasForeignKey(s => s.EnterpriseId);
        }
    }
}
