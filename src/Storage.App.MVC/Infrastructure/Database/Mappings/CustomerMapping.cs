using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Customer;

namespace Storage.App.MVC.Infrastructure.Database.Mappings
{
    public class CustomerMapping : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(c => c.LastName)
                   .HasMaxLength(200);

            builder.Property(c => c.Email)
                   .HasMaxLength(200);

            builder.Property(c => c.PhoneNumber)
                   .HasMaxLength(20);

            builder.HasOne(c => c.Enterprise)
                   .WithMany(e => e.Customers)
                   .HasForeignKey(c => c.EnterpriseId);

            builder.HasMany(c => c.Sales)
                   .WithOne(s => s.Customer)
                   .HasForeignKey(s => s.CustomerId);
        }
    }
}
