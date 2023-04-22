using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Enterprise;

namespace Storage.App.MVC.Infrastructure.Database.Mappings
{
    public sealed class ProductMapping : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Count).IsRequired();

            builder.Property(p => p.Price).IsRequired();

            builder.Property(p => p.Brand).HasMaxLength(200);

            builder.HasOne(p => p.Enterprise)
                   .WithMany(e => e.Products)
                   .HasForeignKey(p => p.EnterpriseId);
        }
    }
}
