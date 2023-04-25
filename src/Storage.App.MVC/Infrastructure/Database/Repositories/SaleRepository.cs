using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public sealed class SaleRepository : ISaleRepository
    {
        private readonly SqlServerContext _context;

        public SaleRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SaleEntity>> GetAllAsync(Guid enterpriseId, CancellationToken cancellationToken)
        {
            var sales = await _context.Sales.Where(s => s.EnterpriseId == enterpriseId)
                                            .Include(s => s.Customer)
                                            .Include(s => s.Enterprise)
                                            .Include(s => s.SaleItems)
                                            .ToListAsync(cancellationToken);

            return sales.Count > 0 ? sales : Enumerable.Empty<SaleEntity>();
        }

        public async Task<SaleEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var sale = await _context.Sales.Include(s => s.Customer)
                                            .Include(s => s.Enterprise)
                                            .Include(s => s.SaleItems)
                                            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            return sale ?? new SaleEntity { Id = Guid.Empty };
        }

        public async Task<SaleEntity> CreateAsync(SaleEntity sale, CancellationToken cancellationToken)
        {
            sale.Id = Guid.NewGuid();

            await _context.AddAsync(sale, cancellationToken);

            return sale;
        }

        public async Task UpdateAsync(SaleEntity sale, CancellationToken cancellationToken)
        {
            await Task.Run(() => _context.Update(sale), cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Sales.AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(SaleEntity sale, CancellationToken cancellationToken)
        {
            await Task.Run(() => _context.Sales.Remove(sale), cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
