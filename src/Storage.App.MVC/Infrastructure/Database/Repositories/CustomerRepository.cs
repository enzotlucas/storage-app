using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Customer;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly SqlServerContext _context;

        public CustomerRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerEntity>> GetAllAsync(Guid enterpriseId, CancellationToken cancellationToken)
        {
            var customers = await _context.Customers.Where(c => c.EnterpriseId == enterpriseId)
                                                    .Include(s => s.Enterprise)
                                                    .ToListAsync(cancellationToken);

            return customers.Count > 0 ? customers : Enumerable.Empty<CustomerEntity>();
        }

        public async Task<CustomerEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Include(s => s.Enterprise)
                                                   .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            return customer ?? new CustomerEntity { Id = Guid.Empty };
        }

        public async Task<CustomerEntity> CreateAsync(CustomerEntity customer, CancellationToken cancellationToken)
        {
            customer.Id = Guid.NewGuid();

            await _context.AddAsync(customer, cancellationToken);

            return customer;
        }

        public async Task UpdateAsync(CustomerEntity customer, CancellationToken cancellationToken)
        {
            await Task.Run(() => _context.Update(customer), cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Customers.AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(CustomerEntity customer, CancellationToken cancellationToken)
        {
            await Task.Run(() => _context.Customers.Remove(customer), cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
