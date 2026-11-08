using AutoMapper;
using warehouse_api.Domain;
using warehouse_api.DTOs.Bookings;
using warehouse_api.Repositories.Interfaces;
using warehouse_api.Services.Interfaces;

namespace warehouse_api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISpaceRepository _spaceRepository;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepository,
            ISpaceRepository spaceRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _spaceRepository = spaceRepository;
            _mapper = mapper;
        }

        public async Task<List<BookingResponseDto>> GetAllAsync()
        {
            var bookings =
                await _bookingRepository.GetAllAsync();

            return _mapper.Map<List<BookingResponseDto>>(bookings);
        }

        public async Task<BookingResponseDto?> GetByIdAsync(int bookingId)
        {
            var booking =
                await _bookingRepository.GetByIdAsync(
                    bookingId);

            if (booking is null)
            {
                return null;
            }

            return _mapper.Map<BookingResponseDto>(booking);
        }

        public async Task<List<BookingCalendarResponseDto>>
            GetCalendarAsync(
                BookingCalendarRequestDto request)
        {
            return await _bookingRepository.GetCalendarAsync(
                request.WarehouseId,
                request.FromDate,
                request.TillDate);
        }

        // First version does not need too much business logic. The second version will business logic, includes:
        // 1. Check if the space is available for the requested booking date and time.
        //2. Check if space belongs to the warehouse assigned to the employee making the booking.
        // 3. check if space is active or not.
        // 4. how many bookings are already made for the space on the requested date and time.
        // 5. How much square footage is available in the space for the requested booking date and time.
        // 6. Is  the square footage requested for the booking less than or equal to the available square footage in the space for the requested booking date and time.

        // It could be more logic, but for now, we will keep it simple and add more logic as needed.
        public async Task<BookingResponseDto> CreateAsync(CreateBookingRequestDto request)
        {
            
            await ValidateAvailabilityAsync(
                request.WarehouseId,
                request.SpaceId,
                request.BookingFromDate,
                request.BookingTillDate,
                request.SqftAreaNeeded);// if all validations are satisfied which means nothing is thrown, then it will continue to create the booking.


            var booking = _mapper.Map<Booking>(request);

            booking.BookingDate = DateTime.Now;

            var createdBooking = await _bookingRepository.CreateAsync(booking);

            return _mapper.Map<BookingResponseDto>(createdBooking);
        }

        public async Task<BookingResponseDto?> UpdateAsync(int bookingId, UpdateBookingRequestDto request)
        {
            var booking =
                await _bookingRepository.GetByIdAsync(
                    bookingId);

            if (booking is null)
            {
                return null;
            }

            await ValidateAvailabilityAsync(
                request.WarehouseId,
                request.SpaceId,
                request.BookingFromDate,
                request.BookingTillDate,
                request.SqftAreaNeeded,
                bookingId); 


            _mapper.Map(request, booking);

            var updatedBooking =
                await _bookingRepository.UpdateAsync(booking);

            return _mapper.Map<BookingResponseDto>(
                updatedBooking);
        }

        public async Task<bool> DeleteAsync(int bookingId)
        {
            return await _bookingRepository.DeleteAsync(
                bookingId);
        }

        private async Task ValidateAvailabilityAsync 
            (
            int warehouseId,
            int spaceId,
            DateTime fromDate,
            DateTime tillDate,
            int sqftAreaNeeded,
            int? excludedBookingId = null
            )
        {
            var space = await _spaceRepository.GetByIdAsync(spaceId); // Domain of spaces will be used to check, not dto

            if (space is null)
            {
                throw new ArgumentException("Space is not found");
            }
            if (space.WarehouseId != warehouseId)
            {
                throw new ArgumentException(
                    "The selected space does not belong to the selected warehouse.");
            }

            if (!space.IsActive)
            {
                throw new ArgumentException(
                    "The selected space is inactive.");
            }

            var overlappingBookings = await _bookingRepository.GetOverlappingBookingsAsync(warehouseId, fromDate, tillDate);
            var bookedArea = overlappingBookings.Where(
                booking => booking.SpaceId == spaceId &&
                    (!excludedBookingId.HasValue ||
                     booking.BookingId !=
                     excludedBookingId.Value)).Sum(booking => booking.SqftAreaNeeded);

            var availableArea = Math.Max(0, space.AreaSqft - bookedArea);

            if (sqftAreaNeeded > availableArea)
            {
                throw new ArgumentException(
                    $"Only {availableArea} sqft is available for the selected date range.");
            }
        }
    }
}
