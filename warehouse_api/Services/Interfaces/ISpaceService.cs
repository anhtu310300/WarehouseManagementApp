using warehouse_api.DTOs.Spaces;

namespace warehouse_api.Services.Interfaces
{
    public interface ISpaceService
    {
        Task<List<SpaceResponseDto>> GetAllAsync();

        Task<SpaceResponseDto?> GetByIdAsync(int spaceId);

        Task<List<SpaceAvailabilityResponseDto>> GetAvailabilityAsync(int warehouseId, DateTime fromDate, DateTime tillDate);

        Task<SpaceResponseDto> CreateAsync(
            CreateSpaceRequestDto request);

        Task<SpaceResponseDto?> UpdateAsync(
            int spaceId,
            UpdateSpaceRequestDto request);

        Task<bool> DeleteAsync(int spaceId);
    }
}