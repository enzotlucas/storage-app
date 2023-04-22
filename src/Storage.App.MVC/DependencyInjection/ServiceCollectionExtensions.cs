using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Core.Sale;
using Storage.App.MVC.Infrastructure.Database.Repositories;
using Storage.App.MVC.Infrastructure.Database;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.UseCases.ActivityHistory;

namespace Storage.App.MVC.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ActivityHistoryProfile));

            services.AddScoped<IGetActivity, GetActivity>();
            services.AddScoped<IGetActivityById, GetActivityById>();
            services.AddScoped<ISaveActivity, SaveActivity>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlServerContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            });

            services.AddScoped<IActivityHistoryRepository, ActivityHistoryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
