using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.Repositories.Interfaces;

namespace warehouse_api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly WarehouseDbContext _context;

        public EmployeeRepository(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.OrderBy(employee => employee.EmpId).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int employeeId)
        {
            return await _context.Employees.FindAsync(employeeId); // primary key findAsync ok
        }

        public async Task<Employee> CreateAsync(Employee employee)
        { 
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteAsync(int employeeId)
        { 
            var employee = await _context.Employees.FindAsync(employeeId);

            if(employee == null)
            {
                return false;
            }

            employee.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
          



    }
}
