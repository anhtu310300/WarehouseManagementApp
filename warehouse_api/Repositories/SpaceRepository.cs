using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.Repositories.Interfaces;

namespace warehouse_api.Repositories
{
    public class SpaceRepository : ISpaceRepository
    {
        private readonly WarehouseDbContext _context;
        public SpaceRepository(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Space>> GetAllAsync()
        { 
            return await _context.Spaces.OrderBy(space => space.SpaceId).ToListAsync();
        }

        public async Task<Space?> GetByIdAsync(int spaceId) 
        { 
            return await _context.Spaces.FindAsync(spaceId);
        }

        public async Task<List<Space>> GetActiveByWarehouseIdAsync(int warehouseId)
        {
            return await _context.Spaces
                .Where(space =>
                    space.WarehouseId == warehouseId &&
                    space.IsActive)
                .OrderBy(space => space.SpaceId)
                .ToListAsync();
        }

        public async Task<Space> CreateAsync(Space space)
        {
            await _context.Spaces.AddAsync(space);

            await _context.SaveChangesAsync();

            return space;
        }

        public async Task<Space> UpdateAsync(Space space)
        {
            _context.Spaces.Update(space);

            await _context.SaveChangesAsync();

            return space;
        }

        public async Task<bool> DeleteAsync(int spaceId)
        {
            var space =
                await _context.Spaces.FindAsync(spaceId);

            if (space is null)
            {
                return false;
            }

            space.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }


    }
}
