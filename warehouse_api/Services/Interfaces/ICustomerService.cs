using warehouse_api.DTOs.Customers;

namespace warehouse_api.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerResponseDto>> GetAllAsync();

        Task<CustomerResponseDto?> GetByIdAsync(
            int customerId);

        Task<CustomerResponseDto> CreateAsync(
            CreateCustomerRequestDto request);

        Task<CustomerResponseDto?> UpdateAsync(
            int customerId,
            UpdateCustomerRequestDto request);

        Task<bool> DeleteAsync(int customerId);
    }
}