using warehouse_api.DTOs.Payments;

namespace warehouse_api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentResponseDto>> GetAllAsync();

        Task<PaymentResponseDto?> GetByIdAsync(int paymentId);

        Task<PaymentResponseDto> CreateAsync(CreatePaymentRequestDto request);

        Task<PaymentResponseDto?> UpdateAsync(int paymentId,UpdatePaymentRequestDto request);

        Task<bool> DeleteAsync(int paymentId);
    }
}