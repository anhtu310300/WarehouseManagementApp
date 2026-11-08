using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.Repositories.Interfaces;

namespace warehouse_api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly WarehouseDbContext _context;

        public CustomerRepository(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .OrderBy(customer => customer.CustomerId)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(
            int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<Customer> CreateAsync(
            Customer customer)
        {
            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> UpdateAsync(
            Customer customer)
        {
            _context.Customers.Update(customer);

            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            var customer =
                await _context.Customers.FindAsync(customerId);

            if (customer is null)
            {
                return false;
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
