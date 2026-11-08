using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.Repositories.Interfaces;

namespace warehouse_api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly WarehouseDbContext _context;

        public AuthRepository(WarehouseDbContext context)
        { 
        _context = context;
        }

        public async Task<Employee?> GetByUserNameAsync(string userName)
        { 
            return await _context.Employees.FirstOrDefaultAsync(employee => employee.UserName == userName);
        }
    }
}
