using warehouse_api.Domain;

namespace warehouse_api.Repositories.Interfaces
{
    public interface ISpaceRepository
    {
        Task<List<Space>> GetAllAsync();

        Task<Space?> GetByIdAsync(int spaceId);

        Task<List<Space>> GetActiveByWarehouseIdAsync(int warehouseId);

        Task<Space> CreateAsync(Space space);

        Task<Space> UpdateAsync(Space space);

        Task<bool> DeleteAsync(int spaceId);
    }
}