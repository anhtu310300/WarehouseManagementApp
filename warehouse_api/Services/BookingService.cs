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

        private async Task ValidateAvailabilityAsync(
    int warehouseId,
    int spaceId,
    DateTime fromDate,
    DateTime tillDate,
    int sqftAreaNeeded,
    int? excludedBookingId = null)
        {
            // 1. check spaces
            var space = await _spaceRepository.GetByIdAsync(spaceId);

            if (space is null)
            {
                throw new ArgumentException("Space is not found.");
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

            // bookings are made for full days, so we only care about the date part of the DateTime.
            var from = fromDate.Date;
            var till = tillDate.Date;

            if (till < from)
            {
                throw new ArgumentException(
                    "Booking end date must be on or after the start date.");
            }

            if (sqftAreaNeeded <= 0)
            {
                throw new ArgumentException(
                    "Required area must be greater than zero.");
            }

            // 2. Get the bookings that overlap with the requested booking date range for the same space.
            var overlappingBookings =
                await _bookingRepository.GetOverlappingBookingsAsync(
                    warehouseId,
                    from,
                    till);

            // same spaces only, and exclude the booking being updated if applicable
            var spaceBookings = overlappingBookings.Where(booking =>
                booking.SpaceId == spaceId &&
                (!excludedBookingId.HasValue ||
                 booking.BookingId != excludedBookingId.Value));

            // 3. Note the date range that square feet taken got changed, and how much square feet got taken on each date in the range.
            var fromDay = DateOnly.FromDateTime(from).DayNumber;
            var tillDay = DateOnly.FromDateTime(till).DayNumber;

            var areaChanges = new SortedDictionary<int, long>();

            foreach (var booking in spaceBookings)
            {
                var bookingFromDay =
                    DateOnly.FromDateTime(booking.BookingFromDate).DayNumber;

                var bookingTillDay =
                    DateOnly.FromDateTime(booking.BookingTillDate).DayNumber;

                // only take part in the date range client wants to book, if the booking is outside the range, it does not affect the availability.
                var startDay = Math.Max(fromDay, bookingFromDay);
                var endDay = Math.Min(tillDay, bookingTillDay);

                if (startDay > endDay)
                {
                    continue;
                }

                // Start date: increase the booked area by the sqftAreaNeeded of the booking.
                areaChanges.TryGetValue(startDay, out var startChange);

                areaChanges[startDay] =
                    startChange + booking.SqftAreaNeeded;

                // End date: decrease the booked area by the sqftAreaNeeded of the booking.
                // Because the space is released only after the end date.
                // Therefore, the area is reduced from the next day.
                var releaseDay = endDay + 1;

                areaChanges.TryGetValue(releaseDay, out var releaseChange);

                areaChanges[releaseDay] =
                    releaseChange - booking.SqftAreaNeeded;
            }

            // 4. find the peak booked area during the requested booking date range
            // by iterating through the areaChanges dictionary and keeping track of the current booked area and the peak booked area.
            long currentBookedArea = 0;
            long peakBookedArea = 0;

            foreach (var change in areaChanges)
            {
                currentBookedArea += change.Value;

                peakBookedArea = Math.Max(
                    peakBookedArea,
                    currentBookedArea);
            }

            // 5. avaialable area during the requested booking date range
            var availableArea = Math.Max(
                0L,
                space.AreaSqft - peakBookedArea);

            // 6. only take request if the requested sqftAreaNeeded is less than or equal to the available area during the requested booking date range.
            if (sqftAreaNeeded > availableArea)
            {
                throw new ArgumentException(
                    $"Only {availableArea} sqft is available for the selected date range.");
            }
        }
    }
}
