using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.Repositories.Interfaces;

namespace warehouse_api.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly WarehouseDbContext _context;

        public WarehouseRepository(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses
                .OrderBy(warehouse => warehouse.WarehouseId)
                .ToListAsync();
        }

        public async Task<Warehouse?> GetByIdAsync(
            int warehouseId)
        {
            return await _context.Warehouses.FindAsync(
                warehouseId);
        }

        public async Task<Warehouse> CreateAsync(
            Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);

            await _context.SaveChangesAsync();

            return warehouse;
        }

        public async Task<Warehouse> UpdateAsync(
            Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);

            await _context.SaveChangesAsync();

            return warehouse;
        }

        public async Task<bool> DeleteAsync(int warehouseId)
        {
            var warehouse =
                await _context.Warehouses.FindAsync(warehouseId);

            if (warehouse is null)
            {
                return false;
            }

            _context.Warehouses.Remove(warehouse);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}