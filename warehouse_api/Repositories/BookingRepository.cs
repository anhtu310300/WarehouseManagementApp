using Microsoft.EntityFrameworkCore;
using warehouse_api.Data;
using warehouse_api.Domain;
using warehouse_api.DTOs.Bookings;
using warehouse_api.Repositories.Interfaces;

namespace warehouse_api.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly WarehouseDbContext _context;

        public BookingRepository(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .OrderBy(booking => booking.BookingId)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(
            int bookingId)
        {
            return await _context.Bookings.FindAsync(
                bookingId);
        }

        public async Task<List<Booking>> GetOverlappingBookingsAsync(int WarehouseId, DateTime fromDate, DateTime tillDate)
        { 
            return await _context.Bookings.Where(booking => booking.WarehouseId == WarehouseId 
            && booking.BookingFromDate <= tillDate 
            && booking.BookingTillDate >= fromDate)
                .ToListAsync();
        }

        public async Task<List<BookingCalendarResponseDto>>
            GetCalendarAsync(
                int warehouseId,
                DateTime fromDate,
                DateTime tillDate)
        {
            return await (
                from booking in _context.Bookings
                join customer in _context.Customers
                    on booking.CustomerId equals customer.CustomerId
                join space in _context.Spaces
                    on booking.SpaceId equals (int?)space.SpaceId
                    into spaceGroup
                from space in spaceGroup.DefaultIfEmpty()
                where booking.WarehouseId == warehouseId
                      && booking.BookingFromDate <= tillDate
                      && booking.BookingTillDate >= fromDate
                orderby booking.BookingFromDate,
                    booking.BookingId
                select new BookingCalendarResponseDto
                {
                    BookingId = booking.BookingId,
                    CustomerId = booking.CustomerId,
                    CustomerName = customer.CustName
                        ?? string.Empty,
                    WarehouseId = booking.WarehouseId,
                    SpaceId = booking.SpaceId,
                    SpaceName = space == null
                        ? null
                        : space.SpaceName,
                    BookingFromDate =
                        booking.BookingFromDate,
                    BookingTillDate =
                        booking.BookingTillDate,
                    SqftAreaNeeded =
                        booking.SqftAreaNeeded,
                    TotalAmount = booking.TotalAmount
                })
                .ToListAsync();
        }

        public async Task<Booking> CreateAsync(
            Booking booking)
        {
            await _context.Bookings.AddAsync(booking);

            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> UpdateAsync(
            Booking booking)
        {
            _context.Bookings.Update(booking);

            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> DeleteAsync(int bookingId)
        {
            var booking =
                await _context.Bookings.FindAsync(bookingId);

            if (booking is null)
            {
                return false;
            }

            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
