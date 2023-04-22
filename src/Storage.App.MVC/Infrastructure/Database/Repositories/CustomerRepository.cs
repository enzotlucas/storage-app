using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Customer;

namespace Storage.App.MVC.Infrastructure.Database.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SqlServerContext _context;

        public CustomerRepository(SqlServerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
        {
            var customers = await _context.Customers.Include(s => s.Enterprise)
                                                    .ToListAsync();

            return customers.Count > 0 ? customers : Enumerable.Empty<CustomerEntity>();
        }

        public async Task<CustomerEntity> GetByIdAsync(Guid id)
        {
            var customer = await _context.Customers.Include(s => s.Enterprise)
                                                   .FirstOrDefaultAsync(s => s.Id == id);

            return customer ?? new CustomerEntity();
        }

        public async Task<CustomerEntity> CreateAsync(CustomerEntity customer)
        {
            customer.Id = Guid.NewGuid();

            await _context.AddAsync(customer);

            return customer;
        }

        public async Task UpdateAsync(CustomerEntity customer)
        {
            await Task.Run(() => _context.Update(customer));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Customers.AnyAsync(s => s.Id == id);
        }

        public async Task DeleteAsync(CustomerEntity customer)
        {
            await Task.Run(() => _context.Customers.Remove(customer));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
