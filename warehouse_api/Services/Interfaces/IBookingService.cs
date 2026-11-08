using warehouse_api.DTOs.Bookings;

namespace warehouse_api.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingResponseDto>> GetAllAsync();

        Task<BookingResponseDto?> GetByIdAsync(
            int bookingId);

        Task<List<BookingCalendarResponseDto>> GetCalendarAsync(
            BookingCalendarRequestDto request);

        Task<BookingResponseDto> CreateAsync(
            CreateBookingRequestDto request);

        Task<BookingResponseDto?> UpdateAsync(
            int bookingId,
            UpdateBookingRequestDto request);

        Task<bool> DeleteAsync(int bookingId);
    }
}
