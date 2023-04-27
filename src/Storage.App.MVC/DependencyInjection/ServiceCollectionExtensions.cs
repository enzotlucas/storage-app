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
using Microsoft.AspNetCore.Identity;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Infrastructure.Identity;
using Storage.App.MVC.Domain.Enterprise;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.UseCases.Enterprise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Storage.App.MVC.Domain.Customer.UseCases;

namespace Storage.App.MVC.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IGetActivity, GetActivity>();
            services.AddScoped<IGetActivityById, GetActivityById>();
            services.AddScoped<IGetActivitiesByObjectId, GetActivitiesByObjectId>();
            services.AddScoped<ISaveActivity, SaveActivity>();

            services.AddScoped<ICreateEnterprise, CreateEnterprise>();
            services.AddScoped<ICreateAdminIfNeeded, CreateAdminIfNeeded>();
            services.AddScoped<IDeleteEnterprise, DeleteEnterprise>();
            services.AddScoped<IGetEnterprises, GetEnterprises>();
            services.AddScoped<IGetEnterpriseById, GetEnterpriseById>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlServerContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            });

            services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
            services.AddScoped<IActivityHistoryRepository, ActivityHistoryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = configuration.GetConnectionString("SqlServerConnection") ?? 
                throw new InvalidOperationException("Connection string 'SqlServerConnection' not found.");

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDefaultIdentity<EnterpriseEntity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Lockout.AllowedForNewUsers = false;
            }).AddEntityFrameworkStores<IdentityContext>();

            return services;
        }
    }
}
