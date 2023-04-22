using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Product;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlServerContext _context;

        public ProductRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            var products = await _context.Products.Include(s => s.Enterprise)
                                                  .ToListAsync();

            return products.Count > 0 ? products : Enumerable.Empty<ProductEntity>();
        }

        public async Task<ProductEntity> GetByIdAsync(Guid id)
        {
            var product = await _context.Products.Include(s => s.Enterprise)
                                                 .FirstOrDefaultAsync(s => s.Id == id);

            return product ?? new ProductEntity();
        }

        public async Task<ProductEntity> CreateAsync(ProductEntity product)
        {
            product.Id = Guid.NewGuid();

            await _context.AddAsync(product);

            return product;
        }

        public async Task UpdateAsync(ProductEntity product)
        {
            await Task.Run(() => _context.Update(product));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Products.AnyAsync(s => s.Id == id);
        }

        public async Task DeleteAsync(ProductEntity product)
        {
            await Task.Run(() => _context.Products.Remove(product));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
