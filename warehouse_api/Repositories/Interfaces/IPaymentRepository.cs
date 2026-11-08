using warehouse_api.Domain;

namespace warehouse_api.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllAsync();

        Task<Payment?> GetByIdAsync(int paymentId);

        Task<List<Payment>> GetByBookingIdAsync(int bookingId); // Get all payments associated with a specific booking ID

        Task<Payment> CreateAsync(Payment payment);

        Task<Payment> UpdateAsync(Payment payment);

        Task<bool> DeleteAsync(int paymentId);
    }
}