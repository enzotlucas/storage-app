using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Domain.Enterprise;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public class EnterpriseRepository : IEnterpriseRepository
    {
        private readonly SqlServerContext _context;

        public EnterpriseRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnterpriseEntity>> GetAllAsync(Guid enterpriseId, CancellationToken cancellationToken)
        {
            var enterprises = await _context.Enterprises.Where(e => e.Name != "Admin").ToListAsync(cancellationToken);

            return enterprises.Count > 0 ? enterprises : Enumerable.Empty<EnterpriseEntity>();
        }

        public async Task<EnterpriseEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var enterprise = await _context.Enterprises.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            return enterprise ?? new EnterpriseEntity { Id = Guid.Empty };
        }

        public async Task<EnterpriseEntity> CreateAsync(EnterpriseEntity enterprise, CancellationToken cancellationToken)
        {
            enterprise.CreatedAt = DateTime.Now;
            enterprise.UpdatedAt = DateTime.Now;

            await _context.AddAsync(enterprise, cancellationToken);

            return enterprise;
        }

        public async Task UpdateAsync(EnterpriseEntity enterprise, CancellationToken cancellationToken)
        {
            enterprise.UpdatedAt = DateTime.Now;

            await Task.Run(() => _context.Update(enterprise), cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Enterprises.AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(EnterpriseEntity enterprise, CancellationToken cancellationToken)
        {
            await Task.Run(() => _context.Enterprises.Remove(enterprise), cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
