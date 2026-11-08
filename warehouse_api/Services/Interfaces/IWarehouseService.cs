using warehouse_api.DTOs.Warehouses;

namespace warehouse_api.Services.Interfaces
{
    public interface IWarehouseService
    {
        Task<List<WarehouseResponseDto>> GetAllAsync();

        Task<WarehouseResponseDto?> GetByIdAsync(
            int warehouseId);

        Task<WarehouseResponseDto> CreateAsync(
            CreateWarehouseRequestDto request);

        Task<WarehouseResponseDto?> UpdateAsync(
            int warehouseId,
            UpdateWarehouseRequestDto request);

        Task<bool> DeleteAsync(int warehouseId);
    }
}
