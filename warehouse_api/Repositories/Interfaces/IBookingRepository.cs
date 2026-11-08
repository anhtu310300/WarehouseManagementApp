using warehouse_api.Domain;

using warehouse_api.DTOs.Bookings;

namespace warehouse_api.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();

        Task<Booking?> GetByIdAsync(int bookingId);

        Task<List<Booking>> GetOverlappingBookingsAsync(int warehouseId, DateTime fromDate, DateTime tillDate);

        Task<List<BookingCalendarResponseDto>> GetCalendarAsync(
            int warehouseId,
            DateTime fromDate,
            DateTime tillDate);

        Task<Booking> CreateAsync(Booking booking);

        Task<Booking> UpdateAsync(Booking booking);

        Task<bool> DeleteAsync(int bookingId);
    }
}
