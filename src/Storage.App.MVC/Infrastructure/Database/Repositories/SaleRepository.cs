using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Sale;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SqlServerContext _context;

        public SaleRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SaleEntity>> GetAllAsync()
        {
            var sales = await _context.Sales.Include(s => s.Customer)
                                            .Include(s => s.Enterprise)
                                            .Include(s => s.SaleItems)
                                            .ToListAsync();

            return sales.Count > 0 ? sales : Enumerable.Empty<SaleEntity>();
        }

        public async Task<SaleEntity> GetByIdAsync(Guid id)
        {
            var sale = await _context.Sales.Include(s => s.Customer)
                                            .Include(s => s.Enterprise)
                                            .Include(s => s.SaleItems)
                                            .FirstOrDefaultAsync(s => s.Id == id);

            return sale ?? new SaleEntity();
        }

        public async Task<SaleEntity> CreateAsync(SaleEntity sale)
        {
            sale.Id = Guid.NewGuid();

            await _context.AddAsync(sale);

            return sale;
        }

        public async Task UpdateAsync(SaleEntity sale)
        {
            await Task.Run(() => _context.Update(sale));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Sales.AnyAsync(s => s.Id == id);
        }

        public async Task DeleteAsync(SaleEntity sale)
        {
            await Task.Run(() => _context.Sales.Remove(sale));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
