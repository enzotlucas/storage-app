using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Product;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly SqlServerContext _context;

        public ProductRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync(Guid enterpriseId, CancellationToken cancellationToken)
        {
            var products = await _context.Products.Where(p => p.EnterpriseId == enterpriseId)
                                                  .Include(s => s.Enterprise)
                                                  .ToListAsync(cancellationToken);

            return products.Count > 0 ? products : Enumerable.Empty<ProductEntity>();
        }

        public async Task<ProductEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.Include(s => s.Enterprise)
                                                 .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            return product ?? new ProductEntity { Id = Guid.Empty };
        }

        public async Task<ProductEntity> CreateAsync(ProductEntity product, CancellationToken cancellationToken)
        {
            product.Id = Guid.NewGuid();

            await _context.AddAsync(product, cancellationToken);

            return product;
        }

        public async Task UpdateAsync(ProductEntity product, CancellationToken cancellationToken)
        {
            await Task.Run(() => _context.Update(product), cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(ProductEntity product, CancellationToken cancellationToken)
        {
            await Task.Run(() => _context.Products.Remove(product), cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
