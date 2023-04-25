using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Enterprise;

namespace Storage.App.MVC.Infrastructure.Identity
{
    public class IdentityContext : IdentityDbContext<EnterpriseEntity>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
    }
}
