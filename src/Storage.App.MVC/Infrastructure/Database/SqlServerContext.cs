using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Infrastructure.Database
{
    public class SqlServerContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<SaleItemEntity> SaleItems { get; set; }
        public DbSet<EnterpriseEntity> Enterprises { get; set; }
        public DbSet<ActivityHistoryEntity> ActivityHistory { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }

        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
